﻿namespace SurveyBasket.API.DTOs.Authentication;

public record _RegisterRequest
(
	string Email,
	string Password,
	string FirstName,
	string LastName
);
