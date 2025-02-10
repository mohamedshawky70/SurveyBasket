using System.Threading;

namespace SurveyBasket.API.Repository.BaseRopository
{
	public interface IBaseRepo<T> where T : class
	{
		public Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
		public Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken=default);
		public Task<T> CreateAsync(T Entity,CancellationToken cancellationToken=default);
		public Task<T> UpdateAsync(T Entity, CancellationToken cancellationToken = default);
		public Task<T> DeleteAsync(T Entity, CancellationToken cancellationToken = default);
	}
}
