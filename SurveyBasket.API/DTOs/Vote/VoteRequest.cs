using SurveyBasket.API.DTOs.VoteAnswer;

namespace SurveyBasket.API.DTOs.Vote;

public record VoteRequest
(
	IEnumerable<VoteAnswerRequest> Answers
);
