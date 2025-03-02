using SurveyBasket.API.DTOs.Authentication;
using SurveyBasket.API.DTOs.User;

namespace SurveyBasket.API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	[Authorize]
	public class AccountController : ControllerBase
	{
		private readonly IAccountService _accountService;

		public AccountController(IAccountService accountService)
		{
			_accountService = accountService;
		}

		[HttpGet("")]
		public async Task<IActionResult> Profile(CancellationToken cancellationToken)
		{
			var userId = User.GetUserId();
			return Ok(await _accountService.Profile(userId!, cancellationToken));
		}
		[HttpPut("")]
		public async Task<IActionResult> Update([FromBody] UpdateProfileRequest request, CancellationToken cancellationToken)
		{
			//إحتمال انه ميرجعش يوزر صفر
			var userId = User.GetUserId();
			return Ok(await _accountService.UpdateAsync(userId!, request, cancellationToken));
		}
		[HttpPut("change-password")]
		public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, CancellationToken cancellationToken)
		{
			var userId = User.GetUserId();
			var response = await _accountService.ChangePasswordAsync(userId!, request, cancellationToken);
			return response.Match(
				Ok,
				error => Problem(error.Code, error.Description, StatusCodes.Status400BadRequest)
				);
		}



	}
}
