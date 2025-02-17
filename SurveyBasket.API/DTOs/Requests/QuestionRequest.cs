namespace SurveyBasket.API.DTOs.Requests;

public record QuestionRequest
(
	int PollId,
	string Content, 
	List<string>answers
);
