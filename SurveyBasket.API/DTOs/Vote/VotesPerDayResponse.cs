namespace SurveyBasket.API.DTOs.Vote;

public record VotesPerDayResponse
(
	DateOnly Date,
	int NumberOfVotes
);