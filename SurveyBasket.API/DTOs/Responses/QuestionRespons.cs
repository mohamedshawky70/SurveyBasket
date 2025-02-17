namespace SurveyBasket.API.DTOs.Responses;

public record QuestionRespons
(
	int Id,
	string Content,
	IEnumerable<AnswerRespons> answers
);
