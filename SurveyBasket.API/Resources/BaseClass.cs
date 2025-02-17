namespace SurveyBasket.API.Resources;

public class BaseClass
{
	public string CreatedById { get; set; } = string.Empty;
	public DateTime CreatedIn { get; set; } = DateTime.UtcNow;
	public string? UpdatedById { get; set; }
	public DateTime UpdatedIn { get; set; }
	public ApplicationUser CreatedBy { get; set; } = default!;
	public ApplicationUser? UpdatedBy { get; set; }
}
