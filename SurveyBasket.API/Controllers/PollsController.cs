

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
				return NotFound();
			var Response = Poll.Adapt<PollResponse>();
			return Ok(Response);
		}

		[HttpPost("")]
		public async Task<IActionResult> CreateAsync([FromBody]PollRequest request,
			CancellationToken cancellationToken)
		{
			var Poll = request.Adapt<Poll>();
			var newPoll = await _unitOfWork.polls.CreateAsync(Poll, cancellationToken);
			_unitOfWork.Commit(cancellationToken);
			return Ok(newPoll);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateAsync([FromRoute]int id,[FromBody] PollRequest request,
			CancellationToken cancellationToken)
		{
			//var Poll = request.Adapt<Poll>();
			var Poll =await _unitOfWork.polls.GetByIdAsync(id);
			if (Poll is null)
				return NotFound();

			Poll.Title = request.Title;
			Poll.Summary = request.Summary;
			Poll.IsPublished = request.IsPublished;
			Poll.StartsAt = request.StartsAt;
			Poll.EndsAt = request.EndsAt;

			var newPoll= await _unitOfWork.polls.UpdateAsync(Poll, cancellationToken);
			_unitOfWork.Commit(cancellationToken);
			return Ok(newPoll);
		}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAsync([FromRoute]int id, CancellationToken cancellationToken)
		{
			var Poll = await _unitOfWork.polls.GetByIdAsync(id, cancellationToken);
			if (Poll is null)
				return NotFound();
			await _unitOfWork.polls.DeleteAsync(Poll, cancellationToken);
			_unitOfWork.Commit(cancellationToken);
			return Ok();
		}
		[HttpPut("TogglePublishStatus/{id}")]
		public async Task<IActionResult> TogglePublishStatus([FromRoute]int id, CancellationToken cancellationToken)
		{
			var Poll = await _unitOfWork.polls.GetByIdAsync(id, cancellationToken);
			if (Poll is null)
				return NotFound();
			Poll.IsPublished = !Poll.IsPublished;
			await _unitOfWork.polls.UpdateAsync(Poll, cancellationToken);
			_unitOfWork.Commit(cancellationToken);
			return Ok();
		}
	}
}
