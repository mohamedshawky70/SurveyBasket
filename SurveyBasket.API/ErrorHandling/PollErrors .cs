namespace SurveyBasket.API.ErrorHandling;

public static class PollErrors
{
	public static readonly Errors NotFound = new Errors("Poll.NotFound", "Poll Is NotFound", StatusCodes.Status404NotFound);
	public static readonly Errors Duplicate = new Errors("Poll.Duplicate", "Poll with the same title already existed", StatusCodes.Status409Conflict);
}
