
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
namespace SurveyBasket.API.Services;
public class CashService<T> : ICashService<T> where T : class
{
	private readonly IDistributedCache _distributedCache;

	public CashService(IDistributedCache distributedCache)
	{
		_distributedCache = distributedCache;
	}

	public async Task<T?> GetAsync(string key, CancellationToken cancellationToken)
	{
		var cachedValue = await _distributedCache.GetStringAsync(key, cancellationToken);
		return cachedValue == null ? null : JsonSerializer.Deserialize<T>(cachedValue);
	}
	public async Task SetAsync(string key, T value, CancellationToken cancellationToken)
	{
		await _distributedCache.SetStringAsync(key, JsonSerializer.Serialize(value), cancellationToken);
	}
	public async Task RemoveAsync(string key, CancellationToken cancellationToken)
	{
		await _distributedCache.RemoveAsync(key, cancellationToken);
	}
}
