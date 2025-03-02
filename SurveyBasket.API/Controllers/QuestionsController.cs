namespace SurveyBasket.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class QuestionsController : ControllerBase
	{
		private readonly IQuestionServices _questionServices;

		public QuestionsController(IQuestionServices questionServices)
		{
			_questionServices = questionServices;
		}

		[HttpGet("{PollId}")]
		public async Task<IActionResult> GetAll([FromRoute] int PollId, CancellationToken cancellationToken)
		{
			var response = await _questionServices.GetAllAsync(PollId, cancellationToken);
			return response.Match(
				response => Ok(response),//لو مرجعش error
				error => Problem(statusCode: error.StatusCode, //لو رجع error
				title: error.Code, detail: error.Description)
			);
		}

		[HttpGet("get-by-id/{id}")]
		public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
		{
			var response = await _questionServices.GetByIdAsync(id, cancellationToken);
			return response.Match(
				response => Ok(response),//لو مرجعش error
				error => Problem(statusCode: error.StatusCode, //لو رجع error
				title: error.Code, detail: error.Description)
			);
		}

		[HttpPost("")]
		public async Task<IActionResult> Create([FromBody] QuestionRequest request, CancellationToken cancellationToken)
		{
			var response = await _questionServices.CreateAsync(request, cancellationToken);
			return response.Match(
				response => Ok(response),//لو مرجعش error
				error => Problem(statusCode: error.StatusCode, //لو رجع error
				title: error.Code, detail: error.Description)
			);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update([FromBody] QuestionRequest request, [FromRoute] int id, CancellationToken cancellationToken)
		{
			var response = await _questionServices.UpdateAsync(request, id, cancellationToken);
			return response.Match(
				response => Ok(response),//لو مرجعش error
				error => Problem(statusCode: error.StatusCode, //لو رجع error
				title: error.Code, detail: error.Description)
			);
		}

		[HttpPut("toggle-active-status/{id}")]
		public async Task<IActionResult> ToggleActiveStatus([FromRoute] int id, CancellationToken cancellationToken)
		{
			var response = await _questionServices.ToggleActiveStatus(id, cancellationToken);
			return response.Match(
				response => Ok(response),//لو مرجعش error
				error => Problem(statusCode: error.StatusCode, //لو رجع error
				title: error.Code, detail: error.Description)
			);
		}
	}
}
