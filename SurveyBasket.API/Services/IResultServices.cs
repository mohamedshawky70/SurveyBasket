namespace SurveyBasket.API.Services;

public interface IResultServices
{
	Task<OneOf<PollVoteResponse, Errors>> GetPollVotesAsync([FromRoute] int pollId, CancellationToken cancellationToken = default);
	Task<OneOf<IEnumerable<VotesPerDayResponse>, Errors>> GetVotesPerDay(int pollId, CancellationToken cancellationToken = default);
	Task<OneOf<IEnumerable<VotesPerQuestionResponse>, Errors>> GetVotesPerQuestionAsync(int pollId, CancellationToken cancellationToken = default);
}
