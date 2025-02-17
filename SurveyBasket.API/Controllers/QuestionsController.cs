using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SurveyBasket.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class QuestionsController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		public QuestionsController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		[HttpGet("{PollId}")]
		public async Task<IActionResult> GetAllAsync([FromRoute] int PollId, CancellationToken cancellationToken)
		{
			var poll = await _unitOfWork.polls.GetByIdAsync(PollId);
			if (poll is null)
				return NotFound(PollErrors.NotFound);

			var questions =await _unitOfWork.questions.FindAllInclude(x => x.PollId == PollId, cancellationToken, new[] { "answers" });
			var response = questions.Adapt<IEnumerable<QuestionResponse>>();
			return Ok(response);
		}

		[HttpGet("GetByIdAsync/{id}")]
		public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken)
		{
			var question = await _unitOfWork.questions.GetByIdAsync(id);
			if (question is null)
				return NotFound(QuestionErrors.NotFound);

			var questions = await _unitOfWork.questions.FindInclude(q => q.Id == id, cancellationToken, new[] { "answers" });

			var response = questions.Adapt<QuestionResponse>();
			return Ok(response);
		}
		

		[HttpPost("")]
		public async Task<IActionResult> CreateAsync([FromBody] QuestionRequest request, CancellationToken cancellationToken)
		{
			var poll = await _unitOfWork.polls.GetByIdAsync(request.PollId);
			if (poll is null)
				return NotFound(PollErrors.NotFound);
			var OldQuestion = await _unitOfWork.questions.FindInclude(q => q.Content == request.Content && q.PollId == request.PollId);
			if (OldQuestion is not null)
				return BadRequest(QuestionErrors.DuplicatedQuestion);

			var question = request.Adapt<Question>();
			await _unitOfWork.questions.CreateAsync(question, cancellationToken);
			var response = question.Adapt<QuestionResponse>();
			return Ok(response);
		}
		
		[HttpPut("{id}")]
		public async Task<IActionResult>UpdateAsync([FromBody] QuestionRequest request,[FromRoute]int id, CancellationToken cancellationToken)
		{
			var question = await _unitOfWork.questions.FindInclude(a => a.Id==id, cancellationToken, new[] { "answers" });
			if (question is null)
				return NotFound(QuestionErrors.NotFound);

			var IsExistedQuestion = await _unitOfWork.questions.FindInclude(a => a.Id != id && 
					a.Content == request.Content && a.PollId == request.PollId);

			if (IsExistedQuestion is not null)
				return BadRequest(QuestionErrors.DuplicatedQuestion);
			question.Content = request.Content;

			var currentAnswers = question.answers.Select(a => a.Content).ToList();
			var newAnswers = request.answers.Except(currentAnswers).ToList();
			//Add new answer
			foreach (var item in newAnswers)
			{
				question.answers.Add(new Answer { Content=item});
			}
			//Delete [soft] not selected answer
			foreach (var item in question.answers)
			{
				//هعدي علي الداتابيز اللي موجود فيها وموجود في الريكويست
				//هيرجع ترو للاكتف واللي مش موجود في الركويست هيرجع فولس للاكتفي
				item.IsActive = request.answers.Contains(item.Content);
			}
		
			await _unitOfWork.questions.UpdateAsync(question, cancellationToken);
			var response = question.Adapt<QuestionResponse>();
			return Ok(response);
		}

		[HttpPut("ToggleActiveStatus/{id}")]
		public async Task<IActionResult> ToggleActiveStatus([FromRoute] int id, CancellationToken cancellationToken)
		{
			var question = await _unitOfWork.questions.GetByIdAsync(id);
			if (question is null)
				return NotFound(QuestionErrors.NotFound);
			question.IsActive = !question.IsActive;
			await _unitOfWork.questions.UpdateAsync(question, cancellationToken);
			return Ok();
		}


	}
}
