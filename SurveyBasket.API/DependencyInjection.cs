using Asp.Versioning;
using Hangfire;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.IdentityModel.Tokens;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using SurveyBasket.API.Data;
using SurveyBasket.API.ExceptionHandler;
using SurveyBasket.API.Settings;
using System.Reflection;
using System.Text;
using System.Threading.RateLimiting;

namespace SurveyBasket.API;

public static class DependencyInjection
{
	public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
	{

		//Add api versioning
		services.AddApiVersioning(options =>
		{
			options.DefaultApiVersion = new ApiVersion(1); //default
			options.AssumeDefaultVersionWhenUnspecified = true; //لو مجلكش فيرجن اعتمد الديفولن
			options.ReportApiVersions = true; // اعرضله كل الفيرجنز في الهيدر
			options.ApiVersionReader = new HeaderApiVersionReader("api-version");// the key in header
		}).AddApiExplorer(options =>
		  {
			  options.GroupNameFormat = "'v'V";
			  options.SubstituteApiVersionInUrl = true;
		  });

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
			.AddEntityFrameworkStores<ApplicationDbContext>()
			.AddDefaultTokenProviders();

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

		//Add IOptions
		services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
		//علشان يشغل الفليديشن علي الكلاس علشان ممكن اغلط وادخل قيم غير منطقيه زي وقت انتهاء التوكن بالسالب او مدخلهوش اصلا
		services.AddOptions<JwtSettings>().BindConfiguration(nameof(JwtSettings)).ValidateDataAnnotations();
		//مش هعرف اعمل انجكت هنا علشان ستاتك كلاس فالحل ده علشان اعرف اجيب قيم الكلاس من السكشن علشان استخدمهم هنا
		var settings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();

		//Add exceptionHandler
		services.AddExceptionHandler<GlobalExceptionHandler>();
		services.AddProblemDetails();

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
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key)),
				ValidIssuer = settings.Issuer,
				ValidAudience = settings.Audience,
			};
		});
		//Add CORS
		services.AddCors(); //default policy

		//Add Distributed Caching
		services.AddDistributedMemoryCache();
		services.AddScoped(typeof(ICashService<>), typeof(CashService<>));

		//Configure Identity
		services.Configure<IdentityOptions>(options =>
		{
			//default is 6
			options.Password.RequiredLength = 8;
			//default false
			options.SignIn.RequireConfirmedEmail = true;
			options.User.RequireUniqueEmail = true;

		});

		//Add email sender
		services.Configure<EmailSettings>(configuration.GetSection(nameof(EmailSettings)));
		services.AddTransient<IEmailSender, EmailSender>();
		services.AddHttpContextAccessor();

		//Add Hangfire
		services.AddHangfire(x => x.UseSqlServerStorage(connectionString));
		services.AddHangfireServer();

		//Add Services
		services.AddSingleton<IJwtProvider, JwtProvider>();
		services.AddScoped<IAuthService, AuthService>();
		services.AddScoped<IPollService, PollService>();
		services.AddScoped<IQuestionServices, QuestionServices>();
		services.AddScoped<IResultServices, ResultServices>();
		services.AddScoped<IVoteService, VoteService>();
		services.AddScoped<IAccountService, AccountService>();
		services.AddScoped<IUserService, UserService>();
		services.AddScoped<IRoleService, RoleService>();

		//Add health checks
		services.AddHealthChecks()
			.AddSqlServer(name: "Database", connectionString: connectionString);//Health check on database

		//Add Rate limiting
		services.AddRateLimiter(RateLimiterOption =>
		{
			RateLimiterOption.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

			//بتعمل بارتشن لكل يوزر الكي بتاعه هوه ال ا بي بتاع اليوزر علشان
			//احدد عدد الريكوست اللي يقدر يبعتها علشان لو كان بيعمل اتاك علي الموقع
			//يعني هنا لو بعت اتنين ريكوست مش هستلم منه اي ريكوست غير بعد عشرين ثانيه 
			//لكن لو استخدمت اي نوع من اللمتر مباشرتا كنت هستلم منه اي عدد من الريكوست
			//لانه ده يوزر واحد يعني الريكوست اللي هيخلص هيدخل اللي بعده فالرجردل
			//مش هيفضي من التوكن ولا الويندو هتتملي يالريكويست
			//مشكلة النوع ده اني اليوزر لو استخد في بي ان ال ا بي بتاعه هيتغير
			//فبالتالي عد من اول وجديد ريكوستات حل المشكله دي هو اليوزر لمت اللي تحد ده 
			RateLimiterOption.AddPolicy("IpLimiter", httpContext =>
				//Fixed window limit
				RateLimitPartition.GetFixedWindowLimiter(
					partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
					factory: _ => new FixedWindowRateLimiterOptions
					{
						PermitLimit = 2,
						Window = TimeSpan.FromSeconds(20)
					}
				)
			);
			// الكي بتاعه هوه اليوزر نفسه 
			RateLimiterOption.AddPolicy("UserLimiter", httpContext =>
				//Fixed window limit
				RateLimitPartition.GetFixedWindowLimiter(
					partitionKey: httpContext.User.Identity?.Name?.ToString(),
					factory: _ => new FixedWindowRateLimiterOptions
					{
						PermitLimit = 2,
						Window = TimeSpan.FromSeconds(20)
					}
				)
			);
			//Concurrency limit
			/*RateLimiterOption.AddConcurrencyLimiter("Concurrency", option =>
			{
				option.PermitLimit = 2;
				option.QueueLimit = 1;
				option.QueueProcessingOrder =QueueProcessingOrder.OldestFirst;//Queue هات اقدم واحد 
			}
			);*/
			//Token bucket limit
			/*RateLimiterOption.AddTokenBucketLimiter("Token", option =>
			{
				option.TokenLimit = 2;
				option.QueueLimit = 1;
				option.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;//Queue هات اقدم واحد 
				option.TokensPerPeriod = 2; //add 2 token after 30 second
				option.ReplenishmentPeriod = TimeSpan.FromSeconds(30);
				option.AutoReplenishment = true; // Automatic adding
			}
			);*/
			//Fixed window limit
			/*RateLimiterOption.AddFixedWindowLimiter("fixed", option =>
			{
				option.PermitLimit = 2;
				option.Window = TimeSpan.FromSeconds(20);
				option.QueueLimit = 1;
				option.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;//Queue هات اقدم واحد 
			}
			);*/
			//Sliding window limit
			/*RateLimiterOption.AddSlidingWindowLimiter("sliding", option =>
			{
				option.PermitLimit = 2;
				option.Window = TimeSpan.FromSeconds(20);
				option.SegmentsPerWindow = 2;
				option.QueueLimit = 1;
				option.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;//Queue هات اقدم واحد 
			}
			);*/


		});
		return services;
	}
}
