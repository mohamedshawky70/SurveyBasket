namespace SurveyBasket.API.DTOs.Requests;

public record VoteRequest
(
	IEnumerable<VoteAnswerRequest> Answers
);
