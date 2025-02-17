namespace SurveyBasket.API.Repository.BaseRopository;

public interface IUnitOfWork
{
	public IBaseRepo<Poll> polls { get; }
	public IBaseRepo<Question> questions { get; }
	public IBaseRepo<Vote> votes { get; }
	public IBaseRepo<VoteAnswer> voteAnswers { get; }

}
