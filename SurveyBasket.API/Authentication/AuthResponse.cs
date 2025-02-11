namespace SurveyBasket.API.Authentication;

public record AuthResponse
(
	string id,
	string? Email,
	string FirstName,
	string LastName,
	string Taken,
	int ExpiresIn
);