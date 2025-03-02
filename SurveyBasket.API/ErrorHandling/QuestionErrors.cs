namespace SurveyBasket.API.ErrorHandling;

public static class QuestionErrors
{
	public static readonly Errors DuplicatedQuestion = new Errors("Question.Duplicated", "Question with the same content is already existed", StatusCodes.Status409Conflict);
	public static readonly Errors NotFound = new Errors("Question.NotFound", "Question not found", StatusCodes.Status404NotFound);
}
