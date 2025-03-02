using SurveyBasket.API.Data;

namespace SurveyBasket.API.Repository.BaseRopository;

public class UnitOfWork : IUnitOfWork
{
	private readonly ApplicationDbContext _dbContext;
	public IBaseRepo<Poll> polls { get; }
	public IBaseRepo<Question> questions { get; }

	public IBaseRepo<Vote> votes { get; }

	public IBaseRepo<VoteAnswer> voteAnswers { get; }
	public IBaseRepo<ApplicationUser> ApplicationUser { get; }



	public UnitOfWork(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
		polls = new BaseRepo<Poll>(_dbContext);
		questions = new BaseRepo<Question>(_dbContext);
		votes = new BaseRepo<Vote>(_dbContext);
		voteAnswers = new BaseRepo<VoteAnswer>(_dbContext);
	}

}
