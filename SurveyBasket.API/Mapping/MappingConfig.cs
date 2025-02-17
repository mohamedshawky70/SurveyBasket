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
			config.NewConfig<QusetionRequest, Question>()
				.Map(dest => dest.answers, src => src.answers.Select(answer => new Answer { Content = answer }));
		}
	}
}
