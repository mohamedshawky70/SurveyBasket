namespace SurveyBasket.API.ErrorHandling;

public class Errors
{
	public string Code { get; } = string.Empty;
	public string Description { get; } = string.Empty;
	public Errors(string _code,string _Description)
	{
		Code = _code;
		Description = _Description;
	}

}
