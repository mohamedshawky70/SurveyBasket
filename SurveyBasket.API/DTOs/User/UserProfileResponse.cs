﻿namespace SurveyBasket.API.DTOs.User;
public record UserProfileResponse
(
	string Email,
	string UserName,
	string FirstName,
	string LastName
);
