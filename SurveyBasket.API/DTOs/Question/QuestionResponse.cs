namespace SurveyBasket.API.DTOs.Question;

public record QuestionResponse
(
	int Id,
	string Content,
	IEnumerable<AnswerResponse> answers
);
