namespace SurveyBasket.API.DTOs.Requests;
//Immutable by default
public record PollRequest
(
	 int Id,
	 string Title,
	 string Summary,
	 DateOnly StartsAt,
	 DateOnly EndsAt
);
