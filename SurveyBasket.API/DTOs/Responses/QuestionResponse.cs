using SurveyBasket.API.Resources;

namespace SurveyBasket.API.DTOs.Responses;

public record QuestionResponse
(
	int Id,
	string Content,
	IEnumerable<AnswerResponse> answers 
);
