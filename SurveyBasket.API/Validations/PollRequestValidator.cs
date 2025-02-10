
using FluentValidation;

namespace SurveyBasket.API.Validations
{
	//Client side (CreatePollRequest)
	public class PollRequestValidator : AbstractValidator<PollRequest>
	{
		public PollRequestValidator()
		{
			RuleFor(x => x.Title)
				.NotEmpty()//because [Required] accepted ==> "" but .NotEmpty() don't accepted==>""
						   //.WithMessage("") Message error
						   //.When(x=>x.Title=="mo")// Condition ممكن تديلها اسم فانكشن بتعمل شرط كبير
				.Length(3, 100);
			RuleFor(x => x.Summary)
				.NotEmpty()
				.Length(3, 1500);
			RuleFor(x => x.StartsAt)
				.NotEmpty()
				.GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));
			RuleFor(x => x)
				.Must(HasValidDate)
				.WithName(nameof(PollRequest.EndsAt))// علشان اوضحله الايرور في انهي عمود
				.WithMessage("{PropertyName} must be greater than or equal start date");
		}
		public bool HasValidDate(PollRequest pollRequest)
		{
			return pollRequest.EndsAt >= pollRequest.StartsAt;
		}
	}
}
