namespace SurveyBasket.API.Authentication;

public interface IJwtProvider
{
	(string taken, int expireIn) GenerateTaken(ApplicationUser user, IEnumerable<string> roles);
	//Refresh Taken
	string? ValidateTaken(string taken);
}
