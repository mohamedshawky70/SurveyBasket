using Microsoft.AspNetCore.Identity.UI.Services;

namespace SurveyBasket.API.Services;

public class NotificationService : INotificationService
{
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly IEmailSender _emailSender;
	public NotificationService(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IEmailSender emailSender)
	{
		_userManager = userManager;
		_unitOfWork = unitOfWork;
		_httpContextAccessor = httpContextAccessor;
		_emailSender = emailSender;
	}

	public async Task SendNewPollNotification()
	{
		var polls = await _unitOfWork.polls.FindAllInclude(x => x.IsPublished && x.StartsAt == DateOnly.FromDateTime(DateTime.UtcNow));
		var users = await _userManager.GetUsersInRoleAsync(AppRoles.Member);
		foreach (var poll in polls)//All polls to all users
		{
			foreach (var user in users)
			{
				var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;// اليو ار ال اللي مبعوتلي مع الريكويست في الهيدر
				var TempPath = $"{Directory.GetCurrentDirectory()}/Templates/PollNotification.html";//مكان التمبلت اللي هتتبعت
				StreamReader streamReader = new StreamReader(TempPath);//للتعامل مع هذه التمبلت
				var body = streamReader.ReadToEnd();//إقراها للاخر
				streamReader.Close();
				body = body
				.Replace("{{name}}", $"{user.FirstName} {user.LastName}")
					.Replace("{{pollTill}}", poll.Title)
					.Replace("{{endDate}}", poll.EndsAt.ToString())
					.Replace("{{url}}", $"{origin}/polls/start{poll.Id}");

				await _emailSender.SendEmailAsync(user.Email!, $"New poll{poll.Title}", body);
			}
		}
	}
}
