using SurveyBasket.API.DTOs.Authentication;

namespace SurveyBasket.API.Services;

public interface IAuthService
{
	Task<OneOf<AuthResponse?, Errors>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default);
	Task<OneOf<AuthResponse?, Errors>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
	Task<OneOf<bool, Errors>> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
	Task<OneOf<string?, Errors>> RegisterAsync([FromBody] _RegisterRequest request, CancellationToken cancellationToken = default);
	Task<OneOf<AuthResponse?, Errors>> ConfirmEmailAsync([FromBody] ConfirmEmailRequest request);
	Task<OneOf<string, Errors>> ResendConfirmationEmailAsync([FromBody] _ResendConfirmationEmailRequest request);
	Task<OneOf<string, Errors>> SendResetPasswordAsync([FromBody] ForgetPasswordRequest request);
	Task<OneOf<string, Errors>> ResetPasswordAsync([FromBody] _ResetPasswordRequest request);
}
