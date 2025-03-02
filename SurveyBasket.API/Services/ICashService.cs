namespace SurveyBasket.API.Services;

public interface ICashService<T> where T : class
{
	public Task<T?> GetAsync(string key, CancellationToken cancellationToken);
	public Task SetAsync(string key, T value, CancellationToken cancellationToken);
	public Task RemoveAsync(string key, CancellationToken cancellationToken);
}
