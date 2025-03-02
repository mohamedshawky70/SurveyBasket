namespace SurveyBasket.API.DTOs.Poll;
//Immutable by default
public record PollRequest
(
	 int Id,
	 string Title,
	 string Summary,
	 DateOnly StartsAt,
	 DateOnly EndsAt
);
