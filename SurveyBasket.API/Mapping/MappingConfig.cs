using Mapster;

namespace SurveyBasket.API.Mapping
{
	public class MappingConfig : IRegister
	{
		public void Register(TypeAdapterConfig config)
		{
			//Poll
			config.NewConfig<Poll,PollResponse>();
		}
	}
}
