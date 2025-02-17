
using Microsoft.EntityFrameworkCore;
using SurveyBasket.API.Data;
using System.Threading.Tasks;

namespace SurveyBasket.API.Repository.BaseRopository;

public class UnitOfWork : IUnitOfWork
{
	private readonly ApplicationDbContext _dbContext;
	public IBaseRepo<Poll> polls { get; }
	public IBaseRepo<Question> question { get; }

	public UnitOfWork(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
		polls = new BaseRepo<Poll>(_dbContext);
		question = new BaseRepo<Question>(_dbContext);
	}
	
}
