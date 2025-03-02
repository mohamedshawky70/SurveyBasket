namespace SurveyBasket.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = AppRoles.Member)]
	public class VotesController : ControllerBase
	{
		private readonly IVoteService _VoteService;
		public VotesController(IVoteService voteService)
		{
			_VoteService = voteService;
		}

		[HttpGet("{pollId}")]
		public async Task<IActionResult> Start([FromRoute] int pollId, CancellationToken cancellationToken)
		{
			var userId = User.GetUserId();
			var Response = await _VoteService.StartAsync(pollId, userId!, cancellationToken);
			return Response.Match(
				Response => Ok(Response),//لو مرجعش error
				error => Problem(statusCode: error.StatusCode, //لو رجع error
				title: error.Code, detail: error.Description)
			);
		}

		[HttpPost("{pollId}")]
		public async Task<IActionResult> Create([FromRoute] int pollId, VoteRequest request, CancellationToken cancellationToken)
		{
			var userId = User.GetUserId();
			var Response = await _VoteService.CreateAsync(pollId, userId!, request, cancellationToken);
			return Response.Match(
				Response => Ok(Response),//لو مرجعش error
				error => Problem(statusCode: error.StatusCode, //لو رجع error
				title: error.Code, detail: error.Description)
			);
		}
	}
}
