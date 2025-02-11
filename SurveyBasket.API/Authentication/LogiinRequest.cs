namespace SurveyBasket.API.Authentication;

public record LogiinRequest
(
	string email,
	string password
);
