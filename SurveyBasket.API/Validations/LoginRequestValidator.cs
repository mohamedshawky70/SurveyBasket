
namespace SurveyBasket.API.Validations;

public class LoginRequestValidator:AbstractValidator<LogiinRequest>
{
	public LoginRequestValidator()
	{
		RuleFor(x => x.email)
			.NotEmpty()
			.EmailAddress();
		RuleFor(x => x.password)
			.NotEmpty();
	}
}
