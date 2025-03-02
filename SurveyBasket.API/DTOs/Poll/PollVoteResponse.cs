namespace SurveyBasket.API.DTOs.Poll;

public record PollVoteResponse
(
	string Title,
	IEnumerable<VoteResponse> Votes
);
