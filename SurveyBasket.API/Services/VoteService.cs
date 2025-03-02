namespace SurveyBasket.API.Services;

public class VoteService : IVoteService
{
	private readonly IUnitOfWork _unitOfWork;

	public VoteService(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}
	public async Task<OneOf<IEnumerable<QuestionResponse>, Errors>> StartAsync(int pollId, string userId, CancellationToken cancellationToken = default)
	{
		var hasVot = await _unitOfWork.votes.FindInclude(v => v.PollId == pollId && v.UserId == userId, cancellationToken);
		if (hasVot is not null)
			return VoteErrors.DuplicatedVote;

		var poll = await _unitOfWork.polls.FindInclude(p => p.Id == pollId && DateOnly.FromDateTime(DateTime.UtcNow) >= p.StartsAt && DateOnly.FromDateTime(DateTime.UtcNow) <= p.EndsAt);
		if (poll is null)
			return PollErrors.NotFound;

		//var questions = await _unitOfWork.questions.FindAllInclude(q => q.PollId == pollId, cancellationToken, ["answer"]);
		var questions = _unitOfWork.questions.GetAll()
			.Where(q => q.IsActive && q.PollId == pollId)
			.Include(q => q!.answers)
			.Select(x => new QuestionResponse(
				x!.Id,
				x.Content,
				x.answers.Where(a => a.IsActive).Select(a => new AnswerResponse(a.Id, a.Content))
				)).AsNoTracking().ToList();

		return questions;
	}
	public async Task<OneOf<Successes, Errors>> CreateAsync(int pollId, string userId, VoteRequest request, CancellationToken cancellationToken)
	{
		var hasVot = await _unitOfWork.votes.FindInclude(v => v.PollId == pollId && v.UserId == userId, cancellationToken);
		if (hasVot is not null)
			return VoteErrors.DuplicatedVote;

		var poll = await _unitOfWork.polls.FindInclude(p => p.Id == pollId && DateOnly.FromDateTime(DateTime.UtcNow) >= p.StartsAt && DateOnly.FromDateTime(DateTime.UtcNow) <= p.EndsAt);
		if (poll is null)
			return PollErrors.NotFound;

		var availableQuestions = await _unitOfWork.questions.GetAll()
			.Where(q => q!.PollId == pollId && q.IsActive)
			.Select(x => x!.Id).ToListAsync();

		var vote = new Vote()
		{
			PollId = pollId,
			UserId = userId!,
			VoteAnswers = request.Answers.Adapt<IEnumerable<VoteAnswer>>().ToList()
		};
		await _unitOfWork.votes.UpdateAsync(vote, cancellationToken);
		return new Successes("Vote Updated successfully");
	}
}
