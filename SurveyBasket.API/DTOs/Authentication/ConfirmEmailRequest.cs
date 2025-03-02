namespace SurveyBasket.API.DTOs.Authentication;

public record ConfirmEmailRequest
(
	string UserId,
	string Code
);
