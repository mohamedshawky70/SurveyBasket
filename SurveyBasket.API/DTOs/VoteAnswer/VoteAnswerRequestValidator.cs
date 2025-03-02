namespace SurveyBasket.API.DTOs.VoteAnswer
{
	//Client side (CreatePollRequest)
	public class VoteAnswerRequestValidator : AbstractValidator<VoteAnswerRequest>
	{
		public VoteAnswerRequestValidator()
		{
			RuleFor(x => x.QuestionId)
				.GreaterThan(0);
			RuleFor(x => x.AnswerId)
				.GreaterThan(0);
			//ابن لفوت ريكوست اذن لازم تفهم ابوه اني بعمل فاليديت علي ابنك 
		}

	}

}
