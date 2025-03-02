namespace SurveyBasket.API.Services;

public interface IPollService
{
	Task<IEnumerable<PollResponse>> GetAllAsync();
	Task<OneOf<PollResponse, Errors>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
	Task<IEnumerable<PollResponse>> GetCurrentAsyncV1(CancellationToken cancellationToken = default);
	//V2
	Task<IEnumerable<PollResponseV2>> GetCurrentAsyncV2(CancellationToken cancellationToken = default);
	Task<OneOf<PollResponse, Errors>> CreateAsync(PollRequest request, CancellationToken cancellationToken = default);
	Task<OneOf<PollResponse, Errors>> UpdateAsync(int id, PollRequest request, CancellationToken cancellationToken = default);
	Task<OneOf<Successes, Errors>> DeleteAsync(int id, CancellationToken cancellationToken = default);
	Task<OneOf<Successes, Errors>> TogglePublishStatus(int id, CancellationToken cancellationToken = default);
}
