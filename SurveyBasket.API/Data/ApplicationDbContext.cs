using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.API.Resources;
using System.Reflection;
using System.Security.Claims;

namespace SurveyBasket.API.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
	//already registered by default
	private readonly IHttpContextAccessor _httpContextAccessor;
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
	{
		_httpContextAccessor = httpContextAccessor;
	}
	public DbSet<Poll> polls { get; set; }
	public DbSet<Question> questions { get; set; }
	public DbSet<Answer> answers { get; set; }
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		//أي كلاس يبانبلمنت الانرفيس (أ انتتي كونفجريشن)هيضيفه هنا بدل مكنت هضيف عشروميت كلاس لو عندي عشروميت تابل 
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		base.OnModelCreating(modelBuilder);

		var CascadeFks = modelBuilder.Model.GetEntityTypes().SelectMany(t => t.GetForeignKeys())
			.Where(fk => fk.DeleteBehavior == DeleteBehavior.Cascade && !fk.IsOwnership);
		foreach (var fk in CascadeFks)
		{
			fk.DeleteBehavior = DeleteBehavior.Restrict;
		}
	}
	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		//بدل مكنت كل مره هتضيف فيها ضيف معاك اليوز اللي ضاف وامتي , واللي حدث وامتي
		var CurrentUserId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
		var entries = ChangeTracker.Entries<BaseClass>();//لو بيحصل تغير علي اي جدول بيورث البيز,هاتهم كلهم
		foreach (var entity in entries)
		{
			if (entity.State == EntityState.Added)//لو بتضيف
				entity.Property(x => x.CreatedById).CurrentValue = CurrentUserId;
			if (entity.State == EntityState.Modified)//لو بتحدث
			{
				entity.Property(x => x.UpdatedById).CurrentValue = CurrentUserId;
				entity.Property(x => x.UpdatedIn).CurrentValue =DateTime.UtcNow;
			}
		}
		return base.SaveChangesAsync(cancellationToken);
	}
}
