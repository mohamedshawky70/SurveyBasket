namespace SurveyBasket.API.ErrorHandling;

public static class VoteErrors
{
	public static readonly Errors DuplicatedVote = new Errors("Vote.Duplicated", "This user already voted for this poll", StatusCodes.Status409Conflict);
}
