namespace SurveyBasket.API.DTOs.Requests;

public record VoteAnswerRequest
(
	int QuestionId,
	int AnswerId
);
