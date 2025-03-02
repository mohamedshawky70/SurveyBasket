namespace SurveyBasket.API.Services;

public interface IQuestionServices
{
	Task<OneOf<IEnumerable<QuestionResponse>, Errors>> GetAllAsync(int PollId, CancellationToken cancellationToken = default);
	Task<OneOf<QuestionResponse, Errors>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
	Task<OneOf<QuestionResponse, Errors>> CreateAsync(QuestionRequest request, CancellationToken cancellationToken = default);
	Task<OneOf<QuestionResponse, Errors>> UpdateAsync(QuestionRequest request, [FromRoute] int id, CancellationToken cancellationToken = default);
	Task<OneOf<Successes, Errors>> ToggleActiveStatus([FromRoute] int id, CancellationToken cancellationToken = default);
}
