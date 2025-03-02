namespace SurveyBasket.API.DTOs.VoteAnswer;

public record VoteAnswerRequest
(
	int QuestionId,
	int AnswerId
);
