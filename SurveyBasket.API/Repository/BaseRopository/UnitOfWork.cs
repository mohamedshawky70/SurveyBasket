
using Microsoft.EntityFrameworkCore;
using SurveyBasket.API.Data;
using System.Threading.Tasks;

namespace SurveyBasket.API.Repository.BaseRopository;

public class UnitOfWork : IUnitOfWork
{
	private readonly ApplicationDbContext _dbContext;
	public IBaseRepo<Poll> polls { get; }
	public UnitOfWork(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
		polls = new BaseRepo<Poll>(_dbContext);

	}
	public async Task<int> Commit(CancellationToken cancellationToken)
	{
		 return await _dbContext.SaveChangesAsync(cancellationToken);
	}
}
