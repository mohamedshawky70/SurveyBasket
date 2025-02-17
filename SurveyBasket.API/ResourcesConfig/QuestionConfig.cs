
namespace SurveyBasket.API.ResourcesConfig;

public class QuestionConfig : IEntityTypeConfiguration<Question>
{
	public void Configure(EntityTypeBuilder<Question> builder)
	{
		builder.Property(x => x.Content).HasMaxLength(1500);
		builder.HasIndex(x => new { x.Content, x.PollId }).IsUnique();
	}
}
