using Mapster;

namespace SurveyBasket.API.Mapping
{
	public class MappingConfig : IRegister
	{
		public void Register(TypeAdapterConfig config)
		{
			//Poll
			config.NewConfig<Poll,PollResponse>();
			//Question
			config.NewConfig<QuestionRequest, Question>()
				.Map(dest => dest.answers, src => src.answers.Select(answer => new Answer { Content = answer }));
/*			config.NewConfig<Question, QuestionResponse>()
				.Map(dest => dest.answers, src => src.answers.Select(answer=>new Answer {Id=answer.Id, Content = answer.Content }));*/
		}
	}
}
