namespace SurveyBasket.API.DTOs.Vote;

public record VotesPerAnswerResponse
(
	string Answer,
	int count
);
