namespace SurveyBasket.API.DTOs.Authentication;

public class ResetPasswordRequestValidator : AbstractValidator<_ResetPasswordRequest>
{
	public ResetPasswordRequestValidator()
	{
		RuleFor(x => x.Email).NotEmpty().EmailAddress();
		RuleFor(x => x.Code).NotEmpty();
		RuleFor(x => x.newPassword)
			.Matches(RejexPattern.StrongPassword)
			.WithMessage("Password must contains atleast 8 digits, one Uppercase,one Lowercase and NunAlphanumeric");


	}
}
