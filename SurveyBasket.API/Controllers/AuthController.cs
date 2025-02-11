using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace SurveyBasket.API.Controllers
{
    [Route("[controller]")] 
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}

		[HttpPost]
        public async Task<IActionResult> LoginAsync(LogiinRequest request,CancellationToken cancellationToken)
        {
            var authResult=await _authService.GetTakenAsync(request.email, request.password, cancellationToken);
            return authResult is null ?BadRequest("Ivalid email or password") : Ok(authResult);
		}
    }
}
