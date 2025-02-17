namespace SurveyBasket.API.Resources;

public class Question:BaseClass
{
	public int Id { get; set; }
	public string Content { get; set; } = string.Empty;
	public bool IsActive { get; set; } = true; //Soft delete
	public int PollId { get; set; }
	public Poll Poll { get; set; }
	public ICollection<Answer> answers { get; set; } = [];

}
