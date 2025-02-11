namespace SurveyBasket.API.Authentication;

public interface IAuthService
{
	public Task<AuthResponse> GetTakenAsync(string email, string password, CancellationToken cancellationToken = default);
}
