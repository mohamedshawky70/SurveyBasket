namespace SurveyBasket.API.DTOs.User;

public record UserResponse
(
	string Id,
	string FirstName,
	string LastName,
	string Email,
	bool IsDisable,
	IEnumerable<string> Roles
);
