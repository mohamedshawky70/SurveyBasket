namespace SurveyBasket.API.Services;

public interface IVoteService
{
	Task<OneOf<IEnumerable<QuestionResponse>, Errors>> StartAsync(int pollId, string userId, CancellationToken cancellationToken = default);
	Task<OneOf<Successes, Errors>> CreateAsync(int pollId, string userId, VoteRequest request, CancellationToken cancellationToken);
}
