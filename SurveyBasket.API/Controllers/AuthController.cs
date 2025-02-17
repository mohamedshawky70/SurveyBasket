using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.API.ErrorHandling;


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
			//throw new Exception("My Exception");
			var authResult=await _authService.GetTakenAsync(request.email, request.password, cancellationToken);
            //return authResult is null ?BadRequest("Invalid email or password") : Ok(authResult);ده صح واللي تحت الاصح
            return authResult.Match(
                authResult => Ok(authResult),//لو مرجعش error
                error => Problem(statusCode: StatusCodes.Status400BadRequest, //لو رجع error
				title: error.Code, detail: error.Description)
            );
		}
    }
}
