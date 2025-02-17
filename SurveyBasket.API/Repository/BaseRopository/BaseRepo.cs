using SurveyBasket.API.Data;
using SurveyBasket.API.Resource;
using System.Linq.Expressions;

namespace SurveyBasket.API.Repository.BaseRopository
{
	public class BaseRepo<T> : IBaseRepo<T> where T : class
	{
		private readonly ApplicationDbContext _dbContext;

		public BaseRepo(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken=default) =>
			await _dbContext.Set<T>().AsNoTracking().ToListAsync(cancellationToken);
		public async Task<T?>GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
			await _dbContext.Set<T>().FindAsync(id, cancellationToken);
		public async Task<T> CreateAsync(T Entity, CancellationToken cancellationToken = default)
		{
			await _dbContext.Set<T>().AddAsync(Entity, cancellationToken = default);
			await _dbContext.SaveChangesAsync(cancellationToken);
			return Entity;
		}

		public async Task<T> UpdateAsync(T Entity, CancellationToken cancellationToken = default)
		{
			_dbContext.Set<T>().Update(Entity);
			await _dbContext.SaveChangesAsync(cancellationToken);
			return Entity;
		}
		
		public async Task<T> DeleteAsync(T Entity,CancellationToken cancellationToken = default)
		{
			 var res=_dbContext.Set<T>().Remove(Entity);
			await _dbContext.SaveChangesAsync(cancellationToken);
			return Entity;
		}

		public async Task<T> FindMatch(Expression<Func<T, bool>> match)
		{
			IQueryable<T> obj = _dbContext.Set<T>();
			return await obj.FirstOrDefaultAsync(match);
		}

	}
}
