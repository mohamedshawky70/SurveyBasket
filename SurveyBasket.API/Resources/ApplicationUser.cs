namespace SurveyBasket.API.Resources;

public class ApplicationUser : IdentityUser
{
	public string FirstName { get; set; } = null!;
	public string LastName { get; set; } = null!;
	public bool IsDisable { get; set; }
	public List<RefreshToken> RefreshTokens { get; set; } = [];
}
