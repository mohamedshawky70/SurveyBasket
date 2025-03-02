namespace SurveyBasket.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class ResultsController : ControllerBase
	{
		private readonly IResultServices _resultServices;

		public ResultsController(IResultServices resultServices)
		{
			_resultServices = resultServices;
		}

		[HttpGet("row-data/{pollId}")]
		public async Task<IActionResult> GetPollVotes([FromRoute] int pollId, CancellationToken cancellationToken)
		{
			var Response = await _resultServices.GetPollVotesAsync(pollId, cancellationToken);
			return Response.Match(
				Response => Ok(Response),
				error => Problem(statusCode: error.StatusCode,
				title: error.Code, detail: error.Description)
			);
		}

		[HttpGet("votes-per-day/{pollId}")]
		public async Task<IActionResult> GetVotesPerDay([FromRoute] int pollId, CancellationToken cancellationToken)
		{
			var Response = await _resultServices.GetVotesPerDay(pollId, cancellationToken);
			return Response.Match(
				Response => Ok(Response),
				error => Problem(statusCode: error.StatusCode,
				title: error.Code, detail: error.Description)
			);
		}

		[HttpGet("votes-per-Question/{pollId}")]
		public async Task<IActionResult> GetVotesPerQuestion([FromRoute] int pollId, CancellationToken cancellationToken)
		{
			var Response = await _resultServices.GetVotesPerQuestionAsync(pollId, cancellationToken);
			return Response.Match(
				Response => Ok(Response),
				error => Problem(statusCode: error.StatusCode,
				title: error.Code, detail: error.Description)
			);

		}
	}
}
