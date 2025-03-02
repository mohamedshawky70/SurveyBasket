namespace SurveyBasket.API.Services;

public class PollService : IPollService
{
	private readonly IUnitOfWork _unitOfWork;

	public PollService(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<IEnumerable<PollResponse>> GetAllAsync()
	{
		var Polls = await _unitOfWork.polls.GetAllAsync();
		var Response = Polls.Adapt<IEnumerable<PollResponse>>();
		return Response;
	}
	public async Task<OneOf<PollResponse, Errors>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
	{
		var Poll = await _unitOfWork.polls.GetByIdAsync(id, cancellationToken);
		if (Poll is null)
			return PollErrors.NotFound;
		var Response = Poll.Adapt<PollResponse>();
		return Response;
	}
	public async Task<IEnumerable<PollResponse>> GetCurrentAsyncV1(CancellationToken cancellationToken = default)
	{
		var Poll = await _unitOfWork.polls.FindAllInclude(p => p.IsPublished && DateOnly.FromDateTime(DateTime.UtcNow) >= p.StartsAt && DateOnly.FromDateTime(DateTime.UtcNow) <= p.EndsAt);
		var Response = Poll.Adapt<IEnumerable<PollResponse>>();
		return Response;
	}
	//V2
	public async Task<IEnumerable<PollResponseV2>> GetCurrentAsyncV2(CancellationToken cancellationToken = default)
	{
		var Poll = await _unitOfWork.polls.FindAllInclude(p => p.IsPublished && DateOnly.FromDateTime(DateTime.UtcNow) >= p.StartsAt && DateOnly.FromDateTime(DateTime.UtcNow) <= p.EndsAt);
		var Response = Poll.Adapt<IEnumerable<PollResponseV2>>();
		return Response;
	}
	public async Task<OneOf<PollResponse, Errors>> CreateAsync([FromBody] PollRequest request, CancellationToken cancellationToken = default)
	{
		var IsExistedTitle = await _unitOfWork.polls.FindInclude(x => x.Title == request.Title);
		if (IsExistedTitle != null)
			return PollErrors.Duplicate;
		var Poll = request.Adapt<Poll>();
		var newPoll = await _unitOfWork.polls.CreateAsync(Poll, cancellationToken);
		var Response = newPoll.Adapt<PollResponse>();
		return Response;
	}
	public async Task<OneOf<PollResponse, Errors>> UpdateAsync(int id, PollRequest request, CancellationToken cancellationToken = default)
	{
		var IsExistedTitle = await _unitOfWork.polls.FindInclude(x => x.Title == request.Title && x.Id != id);
		if (IsExistedTitle != null)
			return PollErrors.Duplicate;
		//var Poll = request.Adapt<Poll>();
		var Poll = await _unitOfWork.polls.GetByIdAsync(id);
		if (Poll is null)
			return PollErrors.NotFound;

		Poll = request.Adapt(Poll);
		var newPoll = await _unitOfWork.polls.UpdateAsync(Poll, cancellationToken);
		return newPoll.Adapt<PollResponse>();
	}
	//If you delete poll that has questions ==>welcome to exception because .Restrict
	public async Task<OneOf<Successes, Errors>> DeleteAsync(int id, CancellationToken cancellationToken = default)
	{
		var Poll = await _unitOfWork.polls.GetByIdAsync(id, cancellationToken);
		if (Poll is null)
			return PollErrors.NotFound;
		await _unitOfWork.polls.DeleteAsync(Poll, cancellationToken);
		return new Successes("Poll deleted successfully");
	}
	public async Task<OneOf<Successes, Errors>> TogglePublishStatus(int id, CancellationToken cancellationToken = default)
	{
		var Poll = await _unitOfWork.polls.GetByIdAsync(id, cancellationToken);
		if (Poll is null)
			return PollErrors.NotFound;
		Poll.IsPublished = !Poll.IsPublished;
		await _unitOfWork.polls.UpdateAsync(Poll, cancellationToken);
		return new Successes("Status Toggled successfully");
	}

}
