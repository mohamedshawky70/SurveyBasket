
using SurveyBasket.API.Data.Migrations;
using SurveyBasket.API.Resource;
using System.Threading.Tasks;
namespace SurveyBasket.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class PollsController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IConfiguration _configuration;
		public PollsController(IUnitOfWork unitOfWork, IConfiguration configuration)
		{
			_unitOfWork = unitOfWork;
			_configuration = configuration;
		}

		[HttpGet("")]
		public async Task<IActionResult> GetAllAsync()
		{
			var Polls =await _unitOfWork.polls.GetAllAsync();
			var Response = Polls.Adapt<IEnumerable<PollResponse>>();
			return Ok(Response);
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetByIdAsync([FromRoute]int id, CancellationToken cancellationToken)
		{
			var Poll = await _unitOfWork.polls.GetByIdAsync(id, cancellationToken);
			if (Poll is null)
				return NotFound(PollErrors.NotFound);
			var Response = Poll.Adapt<PollResponse>();
			return Ok(Response);
		}
		
		[HttpGet("GetCurrentAsync")]
		public async Task<IActionResult> GetCurrentAsync(CancellationToken cancellationToken)
		{
			var Poll = await _unitOfWork.polls.FindAllInclude(p => p.IsPublished && DateOnly.FromDateTime(DateTime.UtcNow) >=p.StartsAt&& DateOnly.FromDateTime(DateTime.UtcNow) <= p.EndsAt);
			if (Poll is null)
				return NotFound(PollErrors.NotFound);
			var Response = Poll.Adapt<IEnumerable<PollResponse>>();
			return Ok(Response);
		}

		[HttpPost("")]
		public async Task<IActionResult> CreateAsync([FromBody]PollRequest request,
			CancellationToken cancellationToken)
		{
			var IsExistedTitle = await _unitOfWork.polls.FindInclude(x => x.Title == request.Title);
				if(IsExistedTitle != null)
				   return BadRequest(PollErrors.DuplicatedPollTitle);
			var Poll = request.Adapt<Poll>();
			var newPoll = await _unitOfWork.polls.CreateAsync(Poll, cancellationToken);
			var Response = newPoll.Adapt<PollResponse>();
			return Ok(Response);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateAsync([FromRoute]int id,[FromBody] PollRequest request,
			CancellationToken cancellationToken)
		{
			var IsExistedTitle = await _unitOfWork.polls.FindInclude(x => x.Title == request.Title&&x.Id!= id);
			if (IsExistedTitle != null)
				return BadRequest(PollErrors.DuplicatedPollTitle);
			//var Poll = request.Adapt<Poll>();
			var Poll =await _unitOfWork.polls.GetByIdAsync(id);
			if (Poll is null)
				return NotFound(PollErrors.NotFound);

			Poll.Title = request.Title;
			Poll.Summary = request.Summary;
			Poll.StartsAt = request.StartsAt;
			Poll.EndsAt = request.EndsAt;

			var newPoll= await _unitOfWork.polls.UpdateAsync(Poll, cancellationToken);
			return Ok(newPoll.Adapt<PollResponse>());
		}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAsync([FromRoute]int id, CancellationToken cancellationToken)
		{
			var Poll = await _unitOfWork.polls.GetByIdAsync(id, cancellationToken);
			if (Poll is null)
				return NotFound(PollErrors.NotFound);
			await _unitOfWork.polls.DeleteAsync(Poll, cancellationToken);
			return Ok();
		}
		[HttpPut("TogglePublishStatus/{id}")]
		public async Task<IActionResult> TogglePublishStatus([FromRoute]int id, CancellationToken cancellationToken)
		{
			var Poll = await _unitOfWork.polls.GetByIdAsync(id, cancellationToken);
			if (Poll is null)
				return NotFound(PollErrors.NotFound);
			Poll.IsPublished = !Poll.IsPublished;
			await _unitOfWork.polls.UpdateAsync(Poll, cancellationToken);
			return Ok();
		}
	}
}
