using SurveyBasket.API.DTOs.User;

namespace SurveyBasket.API.Mapping
{
	public class MappingConfig : IRegister
	{
		public void Register(TypeAdapterConfig config)
		{
			//Poll
			config.NewConfig<Poll, PollResponse>();
			//Question
			config.NewConfig<QuestionRequest, Question>()
				/*			config.NewConfig<Question, QuestionResponse>()
								.Map(dest => dest.answers, src => src.answers.Select(answer=>new Answer {Id=answer.Id, Content = answer.Content }));*/
				.Map(dest => dest.answers, src => src.answers.Select(answer => new Answer { Content = answer }));
			//User
			config.NewConfig<(ApplicationUser user, IList<string> roles), UserResponse>()
				.Map(dest => dest.Id, src => src.user.Id)
				.Map(dest => dest.FirstName, src => src.user.FirstName)
				.Map(dest => dest.LastName, src => src.user.LastName)
				.Map(dest => dest.Email, src => src.user.Email)
				.Map(dest => dest.IsDisable, src => src.user.IsDisable)
				.Map(dest => dest.Roles, src => src.roles);

			config.NewConfig<CreateUserRequest, ApplicationUser>()
				.Map(dest => dest.UserName, src => src.Email)
				.Map(dest => dest.EmailConfirmed, src => true);

			config.NewConfig<UpdateUserRequest, ApplicationUser>()
				.Map(dest => dest.UserName, src => src.Email);
			//.Map(dest => dest.EmailConfirmed, src => true);




		}
	}
}
