namespace SurveyBasket.API.DTOs.Question;

public record QuestionRequest
(
	int PollId,
	string Content,
	List<string> answers
);
