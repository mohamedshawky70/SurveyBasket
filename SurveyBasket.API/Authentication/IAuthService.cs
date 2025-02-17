
namespace SurveyBasket.API.Authentication;

public interface IAuthService
{
	public Task<OneOf<AuthResponse?,Errors>> GetTakenAsync(string email, string password, CancellationToken cancellationToken = default);
}
