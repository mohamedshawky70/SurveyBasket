namespace SurveyBasket.API.ResourcesConfig;

public class AnswerConfig : IEntityTypeConfiguration<Answer>
{
	public void Configure(EntityTypeBuilder<Answer> builder)
	{
		builder.Property(x => x.Content).HasMaxLength(1500);
		builder.HasIndex(x => new { x.Content, x.QuestionId }).IsUnique();//اكتر من إجابة لنفس السؤال  //السؤال الواحد يمتلك اكتر من اجابة
	}
}

