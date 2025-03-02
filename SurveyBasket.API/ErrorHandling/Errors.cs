﻿namespace SurveyBasket.API.ErrorHandling;

public class Errors
{
	public string Code { get; } = string.Empty;
	public string Description { get; } = string.Empty;
	public int StatusCode { get; }
	public Errors(string _code, string _Description, int _StatusCode)
	{
		Code = _code;
		Description = _Description;
		StatusCode = _StatusCode;
	}

}
