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
		[HttpPost("")]
		public async Task<IActionResult> CreateAsync([FromBody] QusetionRequest request,CancellationToken cancellationToken)
		{
			var poll =await _unitOfWork.polls.GetByIdAsync(request.PollId);
			if(poll is null)
				return NotFound(PollErrors.NotFound);
			var OldQuestion = await _unitOfWork.question.FindMatch(q => q.Content == request.Content && q.PollId == request.PollId);
			if (OldQuestion is not null)
				return BadRequest(QuestionErrors.DuplicatedQuestion);

			var question = request.Adapt<Question>();
			await _unitOfWork.question.CreateAsync(question, cancellationToken);
			var response =question.Adapt<QuestionRespons>();
			return Ok(response);
		}
	}
}
