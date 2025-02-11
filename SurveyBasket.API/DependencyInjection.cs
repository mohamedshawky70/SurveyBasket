using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using SurveyBasket.API.Authentication;
using SurveyBasket.API.Data;
using SurveyBasket.API.Resources;
using System.Reflection;
using System.Text;

namespace SurveyBasket.API;

public static class DependencyInjection
{
	public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
	{
		//Add Mapster
		var mappingConfig = TypeAdapterConfig.GlobalSettings;
		mappingConfig.Scan(Assembly.GetExecutingAssembly());
		services.AddSingleton<IMapper>(new Mapper(mappingConfig));

		//Add Fluent Validation
		services.AddFluentValidationAutoValidation()
			.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		//Add Database
		var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
		services.AddDbContext<ApplicationDbContext>(options =>
			options.UseSqlServer(connectionString));
		//Add IdentityRole
		services.AddIdentity<ApplicationUser, IdentityRole>()
			.AddEntityFrameworkStores<ApplicationDbContext>();

		//Add UnitOfWork
		services.AddScoped<IUnitOfWork, UnitOfWork>();

		// Add services to the container.
		services.AddControllers();
		// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
		services.AddOpenApi();

		//Add Identity[Endpoint جاهزه] But you can't customize on table user 
		/*services.AddIdentityApiEndpoints<ApplicationUser>()
			.AddEntityFrameworkStores<ApplicationDbContext>();*/
		//app.MapIdentityApi<ApplicationUser>(); In Program.cs

		//Add login(generate JWT)
		services.AddSingleton<IJwtProvider, JwtProvider>();
		services.AddAuthentication(option =>
		{
			option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		}
		)
		.AddJwtBearer(o =>
		{
			o.SaveToken = true;
			o.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = true,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("OvQWhp2kSk9D1RG8rrHI1qLeiKmBxRaz")),
				ValidIssuer = "SurveyBasketApp",
				ValidAudience = "SurveyBasketUsers",
			};
		});
		//Add Auth service
		services.AddScoped<IAuthService, AuthService>();
		return services;
	}
}
