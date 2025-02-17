namespace SurveyBasket.API.DTOs.Requests;

public record QusetionRequest
(
	int PollId,
	string Content, 
	List<string>answers
);
