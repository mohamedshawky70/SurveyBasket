namespace SurveyBasket.API.Resource;
public class Poll : BaseClass
{
	public int Id { get; set; }
	public string Title { get; set; } = null!;
	public string Summary { get; set; } = null!;
	public bool IsPublished { get; set; }
	public DateOnly StartsAt { get; set; }
	public DateOnly EndsAt { get; set; }
	public ICollection<Question> questions { get; set; } = [];
	public ICollection<Vote> Votes { get; set; } = [];

}

