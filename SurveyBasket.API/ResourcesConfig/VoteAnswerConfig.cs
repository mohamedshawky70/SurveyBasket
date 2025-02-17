namespace SurveyBasket.API.ResourcesConfig;

public class VoteAnswerConfig : IEntityTypeConfiguration<VoteAnswer>
{
	public void Configure(EntityTypeBuilder<VoteAnswer> builder)
	{
		builder.HasIndex(x => new { x.VoteId, x.QuestionId }).IsUnique();
	}
}
