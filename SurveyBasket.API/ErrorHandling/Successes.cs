namespace SurveyBasket.API.ErrorHandling;

public class Successes
{
	public string Message { get; set; }

	public Successes(string message)
	{
		Message = message;
	}
}
