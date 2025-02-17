using System.Linq.Expressions;
using System.Threading;

namespace SurveyBasket.API.Repository.BaseRopository
{
	public interface IBaseRepo<T> where T : class
	{
		public Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
		public IQueryable<T?> GetAll();
		public Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken=default);
		public Task<T> FindInclude(Expression<Func<T, bool>> match, CancellationToken cancellationToken = default, string[] Include = null);
		public Task<IEnumerable<T>> FindAllInclude(Expression<Func<T, bool>> match, CancellationToken cancellationToken = default, string[] Include = null);
		public Task<T> CreateAsync(T Entity,CancellationToken cancellationToken=default);
		public Task<T> UpdateAsync(T Entity, CancellationToken cancellationToken = default);
		public Task<T> DeleteAsync(T Entity, CancellationToken cancellationToken = default);
	}
}
