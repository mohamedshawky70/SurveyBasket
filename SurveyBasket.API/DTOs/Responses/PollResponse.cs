namespace SurveyBasket.API.DTOs.Responses;
public record PollResponse
(
	 int Id,
	 string Title,
	 string Summary,
	 DateOnly StartsAt,
	 DateOnly EndsAt
);
