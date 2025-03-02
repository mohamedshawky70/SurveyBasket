using Asp.Versioning;
using Microsoft.AspNetCore.RateLimiting;
namespace SurveyBasket.API.Controllers
{
	[ApiVersion(1, Deprecated = true)]
	[ApiVersion(2)]
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class PollsController : ControllerBase
	{
		private readonly IPollService _pollService;
		public PollsController(IUnitOfWork unitOfWork, IPollService pollService)
		{
			_pollService = pollService;
		}

		[HttpGet("")]
		public async Task<IActionResult> GetAll()
		{
			return Ok(await _pollService.GetAllAsync());
		}


		[HttpGet("{id}")]
		public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
		{
			var Response = await _pollService.GetByIdAsync(id);
			return Response.Match(
				Response => Ok(Response),//لو مرجعش error
				error => Problem(statusCode: error.StatusCode, //لو رجع error
				title: error.Code, detail: error.Description)
			);
		}
		//V1
		[ApiVersion(1)]
		[HttpGet("get-current")]
		[Authorize(Roles = AppRoles.Member)]
		[EnableRateLimiting("UserLimiter")]
		public async Task<IActionResult> GetCurrentV1(CancellationToken cancellationToken)
		{
			var Response = await _pollService.GetCurrentAsyncV1();
			return Ok(Response);
		}
		//V2
		[ApiVersion(2)]
		[HttpGet("get-current")]
		[Authorize(Roles = AppRoles.Member)]
		[EnableRateLimiting("UserLimiter")]
		public async Task<IActionResult> GetCurrentV2(CancellationToken cancellationToken)
		{
			var Response = await _pollService.GetCurrentAsyncV2();
			return Ok(Response);
		}

		[HttpPost("")]
		public async Task<IActionResult> Create([FromBody] PollRequest request, CancellationToken cancellationToken)
		{
			var Response = await _pollService.CreateAsync(request, cancellationToken);
			return Response.Match(
				//Ok شغال
				Response => Ok(Response),//لو مرجعش error 
				error => Problem(statusCode: error.StatusCode, //لو رجع error
				title: error.Code, detail: error.Description)
			);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest request,
			CancellationToken cancellationToken)
		{
			var Response = await _pollService.UpdateAsync(id, request, cancellationToken);
			return Response.Match(
				Response => Ok(Response),
				error => Problem(statusCode: error.StatusCode,
				title: error.Code, detail: error.Description)
			);
		}
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
		{
			var Response = await _pollService.DeleteAsync(id, cancellationToken);
			return Response.Match(
				Response => Ok(Response),
				error => Problem(statusCode: error.StatusCode,
				title: error.Code, detail: error.Description)
			);
		}
		[HttpPut("toggle-publish-status/{id}")]
		public async Task<IActionResult> TogglePublishStatus([FromRoute] int id, CancellationToken cancellationToken)
		{
			var Response = await _pollService.TogglePublishStatus(id, cancellationToken);
			return Response.Match(
				Response => Ok(Response),
				error => Problem(statusCode: error.StatusCode,
				title: error.Code, detail: error.Description)
			);
		}
	}
}
