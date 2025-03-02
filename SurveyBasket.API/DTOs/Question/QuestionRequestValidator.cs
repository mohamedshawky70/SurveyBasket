namespace SurveyBasket.API.DTOs.Question;

public class QuestionRequestValidator : AbstractValidator<QuestionRequest>
{
	public QuestionRequestValidator()
	{
		RuleFor(x => x.Content).NotEmpty().MaximumLength(1500);
		RuleFor(x => x.answers)
			.Must(x => x.Count > 1)
			.WithMessage("Question has at least 2 answer");
		RuleFor(x => x.answers)
			.Must(x => x.Distinct().Count() == x.Count)//a b c ===>3      a b b c ===>3   3 is answerCount and distinctCount
			.WithMessage("You can't add duplicate answer to the same question ");


	}
}
