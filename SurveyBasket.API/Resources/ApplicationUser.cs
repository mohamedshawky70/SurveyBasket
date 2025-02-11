using Microsoft.AspNetCore.Identity;

namespace SurveyBasket.API.Resources;

public class ApplicationUser:IdentityUser
{
	public string FirstName { get; set; } = null!;
	public string LastName { get; set; } = null!;
}
