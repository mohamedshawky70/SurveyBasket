namespace SurveyBasket.API.Services;

public class QuestionServices : IQuestionServices
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICashService<IEnumerable<QuestionResponse>> _cashService;

	private const string cashPrefix = "allQuestion";
	public QuestionServices(IUnitOfWork unitOfWork, ICashService<IEnumerable<QuestionResponse>> cashService)
	{
		_unitOfWork = unitOfWork;
		_cashService = cashService;
	}
	public async Task<OneOf<IEnumerable<QuestionResponse>, Errors>> GetAllAsync(int PollId, CancellationToken cancellationToken = default)
	{
		var poll = await _unitOfWork.polls.GetByIdAsync(PollId);
		if (poll is null)
			return PollErrors.NotFound;
		//مفتاح هعمل الكاش بيه وهحذفه وهجيبه بيه _
		//يونيك فاليو علشان ميحفظش كل الاسأله بكل البولز بتاعتها بنفس المفتاح علشان لما تجيب الاسأله بتاعة بول واحد يجيب بتاعة بول واحد بس
		var cacheKey = $"{cashPrefix}-{PollId}";
		var cachedQuestion = await _cashService.GetAsync(cacheKey, cancellationToken);
		IEnumerable<QuestionResponse> response = [];
		if (cachedQuestion is null)
		{
			var questions = await _unitOfWork.questions.FindAllInclude(x => x.PollId == PollId, cancellationToken, new[] { "answers" });
			response = questions.Adapt<IEnumerable<QuestionResponse>>();
			await _cashService.SetAsync(cacheKey, response, cancellationToken);
		}
		else
		{
			response = cachedQuestion;
		}
		return response.ToList();
		//return OneOf<IEnumerable<QuestionResponse>, Errors>.FromT0(response);//Just with IEnumerable
	}
	public async Task<OneOf<QuestionResponse, Errors>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
	{
		var question = await _unitOfWork.questions.GetByIdAsync(id);
		if (question is null)
			return QuestionErrors.NotFound;

		var questions = await _unitOfWork.questions.FindInclude(q => q.Id == id, cancellationToken, new[] { "answers" });
		var response = questions.Adapt<QuestionResponse>();
		return response;
	}
	public async Task<OneOf<QuestionResponse, Errors>> CreateAsync(QuestionRequest request, CancellationToken cancellationToken = default)
	{
		var poll = await _unitOfWork.polls.GetByIdAsync(request.PollId);
		if (poll is null)
			return PollErrors.NotFound;
		var OldQuestion = await _unitOfWork.questions.FindInclude(q => q.Content == request.Content && q.PollId == request.PollId);
		if (OldQuestion is not null)
			return QuestionErrors.DuplicatedQuestion;

		var question = request.Adapt<Question>();
		await _unitOfWork.questions.CreateAsync(question, cancellationToken);
		var response = question.Adapt<QuestionResponse>();
		await _cashService.RemoveAsync($"{cashPrefix}-{question.PollId}", cancellationToken);
		return response;
	}
	public async Task<OneOf<QuestionResponse, Errors>> UpdateAsync(QuestionRequest request, [FromRoute] int id, CancellationToken cancellationToken = default)
	{
		var question = await _unitOfWork.questions.FindInclude(a => a.Id == id, cancellationToken, new[] { "answers" });
		if (question is null)
			return QuestionErrors.NotFound;

		var IsExistedQuestion = await _unitOfWork.questions.FindInclude(a => a.Id != id &&
				a.Content == request.Content && a.PollId == request.PollId);

		if (IsExistedQuestion is not null)
			return QuestionErrors.DuplicatedQuestion;
		question.Content = request.Content;

		var currentAnswers = question.answers.Select(a => a.Content).ToList();
		var newAnswers = request.answers.Except(currentAnswers).ToList();
		//Add new answer
		foreach (var item in newAnswers)
		{
			question.answers.Add(new Answer { Content = item });
		}
		//Delete [soft] not selected answer
		foreach (var item in question.answers)
		{
			//هعدي علي الداتابيز اللي موجود فيها وموجود في الريكويست
			//هيرجع ترو للاكتف واللي مش موجود في الركويست هيرجع فولس للاكتفي
			item.IsActive = request.answers.Contains(item.Content);
		}

		await _unitOfWork.questions.UpdateAsync(question, cancellationToken);
		var response = question.Adapt<QuestionResponse>();

		await _cashService.RemoveAsync($"{cashPrefix}-{question.PollId}", cancellationToken);
		return response;
	}
	public async Task<OneOf<Successes, Errors>> ToggleActiveStatus([FromRoute] int id, CancellationToken cancellationToken = default)
	{
		var question = await _unitOfWork.questions.GetByIdAsync(id);
		if (question is null)
			return QuestionErrors.NotFound;
		question.IsActive = !question.IsActive;
		await _unitOfWork.questions.UpdateAsync(question, cancellationToken);
		await _cashService.RemoveAsync($"{cashPrefix}-{question.PollId}", cancellationToken);
		return new Successes("Question of active status toggled successfully");
	}
}
