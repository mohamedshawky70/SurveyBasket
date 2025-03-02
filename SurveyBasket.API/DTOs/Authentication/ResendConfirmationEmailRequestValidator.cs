namespace SurveyBasket.API.DTOs.Authentication;

public class ResendConfirmationEmailRequestValidator : AbstractValidator<ResendConfirmationEmailRequest>
{
	public ResendConfirmationEmailRequestValidator()
	{
		RuleFor(x => x.Email).NotEmpty().EmailAddress();
	}
}
