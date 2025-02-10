namespace SurveyBasket.API.DTOs.Requests;
//Immutable by default
public record PollRequest
(
	 int Id,
	 string Title,
	 string Summary,
	 bool IsPublished,
	 DateOnly StartsAt,
	 DateOnly EndsAt
);
