
using System.Security.Cryptography;

namespace SurveyBasket.API.Authentication;
public class AuthService : IAuthService
{
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly IJwtProvider _jwtProvider;
	public AuthService(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider)
	{
		_userManager = userManager;
		_jwtProvider = jwtProvider;
	}
	public async Task<OneOf<AuthResponse?, Errors>> GetTakenAsync(string email, string password, CancellationToken cancellationToken = default)
	{
		//check email
		var user =await _userManager.FindByEmailAsync(email);
		if (user is null)
			return UserErrors.InvalidCredential;

		//check password
		var IsValidPass=await _userManager.CheckPasswordAsync(user, password);
		if (!IsValidPass)
			return UserErrors.InvalidCredential;
		//generate JWT taken
		var (taken, expiresIn) = _jwtProvider.GenerateTaken(user);
		return new AuthResponse(user.Id,user.Email,user.FirstName,user.LastName, taken, expiresIn);
		
	}
}
