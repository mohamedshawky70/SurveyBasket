namespace SurveyBasket.API.DTOs.Authentication;

public record ChangePasswordRequest
(
	string currentPassword,
	string newPassword
);
