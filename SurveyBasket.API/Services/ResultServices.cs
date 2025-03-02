namespace SurveyBasket.API.Services;

public class ResultServices : IResultServices
{
	private readonly IUnitOfWork _unitOfWork;

	public ResultServices(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}
	public async Task<OneOf<PollVoteResponse, Errors>> GetPollVotesAsync(int pollId, CancellationToken cancellationToken = default)
	{
		var pollVotes = await _unitOfWork.polls.GetAll()
		//Manually mapping علشان متصعبهاش
		.Where(x => x!.Id == pollId)
		.Select(x => new PollVoteResponse(
			x!.Title,
			x.Votes.Select(v => new VoteResponse(
				$"{v.User.FirstName} {v.User.LastName}",
				v.SubmittedOn,
				v.VoteAnswers.Select(a => new QuestionAnswerResponse(
					a.Question.Content,
					a.Answer.Content
				))
			))
		))
		.SingleOrDefaultAsync(cancellationToken);
		return pollVotes == null ? PollErrors.NotFound : pollVotes;
	}
	public async Task<OneOf<IEnumerable<VotesPerDayResponse>, Errors>> GetVotesPerDay(int pollId, CancellationToken cancellationToken = default)
	{
		var poll = await _unitOfWork.polls.FindInclude(x => x.Id == pollId, cancellationToken);
		if (poll is null)
			return PollErrors.NotFound;
		var votesPerDay = _unitOfWork.votes.GetAll()
			.Where(v => v!.PollId == pollId)
			.GroupBy(v => DateOnly.FromDateTime(v!.SubmittedOn))
			.Select(g => new VotesPerDayResponse(g.Key, g.Count()))
			.ToList();
		return votesPerDay;
	}
	public async Task<OneOf<IEnumerable<VotesPerQuestionResponse>, Errors>> GetVotesPerQuestionAsync(int pollId, CancellationToken cancellationToken = default)
	{
		var poll = await _unitOfWork.polls.FindInclude(x => x.Id == pollId, cancellationToken);
		if (poll is null)
			return PollErrors.NotFound;
		var votesPerDay = _unitOfWork.voteAnswers.GetAll()
			.Where(v => v!.Vote.PollId == pollId)
			.Select(v => new VotesPerQuestionResponse(
				v!.Question.Content,
				v.Question.votes
				.GroupBy(v => v.Answer.Content)
				.Select(g => new VotesPerAnswerResponse(
					g.Key,
					g.Count()
					))
			)).ToList();
		return votesPerDay;
	}
}
