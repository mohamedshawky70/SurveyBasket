namespace SurveyBasket.API.ErrorHandling;

public static class UserErrors
{
	public static readonly Errors InvalidCredential = new Errors("User.InvalidCredential", "Invalid email/password"); 
}
