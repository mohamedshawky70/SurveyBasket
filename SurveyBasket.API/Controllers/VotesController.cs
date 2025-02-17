using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.API.Resources;
using System.Threading;
namespace SurveyBasket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotesController : ControllerBase
    {
		private readonly IUnitOfWork _unitOfWork;

		public VotesController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		[HttpGet("{pollId}")]
		public async Task<IActionResult> Start([FromRoute] int pollId, CancellationToken cancellationToken)
		{
			var userId = User.GetUserId();
			var hasVot = await _unitOfWork.votes.FindInclude(v => v.PollId == pollId && v.UserId == userId, cancellationToken);
			if (hasVot is not null)
				return BadRequest(VoteErrors.DuplicatedVote);

			var poll = await _unitOfWork.polls.FindInclude(p => p.Id == pollId && DateOnly.FromDateTime(DateTime.UtcNow) >= p.StartsAt && DateOnly.FromDateTime(DateTime.UtcNow) <= p.EndsAt);
			if (poll is null)
				return NotFound(PollErrors.NotFound);

			//var questions = await _unitOfWork.questions.FindAllInclude(q => q.PollId == pollId, cancellationToken, ["answer"]);
			var questions = _unitOfWork.questions.GetAll()
				.Where(q => q.IsActive && q.PollId == pollId)
				.Include(q => q.answers)
				.Select(x => new QuestionResponse(
					x.Id,
					x.Content,
					x.answers.Where(a => a.IsActive).Select(a => new AnswerResponse(a.Id, a.Content))
					)).AsNoTracking().ToList();

			return Ok(questions);
		}

		[HttpPost("{pollId}")]
		public async Task<IActionResult> CreateAsync([FromRoute] int pollId, VoteRequest request,CancellationToken cancellationToken)
		{
			var userId = User.GetUserId();
			var hasVot = await _unitOfWork.votes.FindInclude(v => v.PollId == pollId && v.UserId == userId, cancellationToken);
			if (hasVot is not null)
				return BadRequest(VoteErrors.DuplicatedVote);

			var poll = await _unitOfWork.polls.FindInclude(p => p.Id == pollId && DateOnly.FromDateTime(DateTime.UtcNow) >= p.StartsAt && DateOnly.FromDateTime(DateTime.UtcNow) <= p.EndsAt);
			if (poll is null)
				return NotFound(PollErrors.NotFound);

			var availableQuestions = await _unitOfWork.questions.GetAll()
				.Where(q => q!.PollId == pollId && q.IsActive)
				.Select(x => x!.Id).ToListAsync();

			var vote = new Vote()
			{
				PollId = pollId,
				UserId = userId!,
				VoteAnswers = request.Answers.Adapt<IEnumerable<VoteAnswer>>().ToList()
			};
			await _unitOfWork.votes.UpdateAsync(vote, cancellationToken);
			return Ok();
		}
	}
}
