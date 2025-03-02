namespace SurveyBasket.API.DTOs.Authentication;

public record _LoginRequest
(
	string email,
	string password
);
