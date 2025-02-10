
namespace SurveyBasket.API.ResourcesConfig;

//In darabase (Server Side) (Poll)
public class PollConfig : IEntityTypeConfiguration<Poll>
{
	public void Configure(EntityTypeBuilder<Poll> builder)
	{
		builder.Property(x => x.Title).HasMaxLength(100);
		builder.HasIndex(x => x.Title).IsUnique();

		builder.Property(x => x.Summary).HasMaxLength(1500);
	}
}
