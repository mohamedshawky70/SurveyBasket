using Hangfire;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using SurveyBasket.API;
using SurveyBasket.API.SeedRoles;
var builder = WebApplication.CreateBuilder(args);

//Start:7/2/2025
//End:2/3/2025
builder.Services.AddDependencies(builder.Configuration);

builder.Services.AddScoped<INotificationService, NotificationService>();
//Add Serilog
builder.Host.UseSerilog((context, configuration) =>
	configuration.ReadFrom.Configuration(context.Configuration)
);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "v1"));
}

//Add Serilog 
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
//Use CORS
app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthorization();

//app.MapIdentityApi<ApplicationUser>();

app.MapControllers();

//Health checks endpoint 
app.MapHealthChecks("health", new HealthCheckOptions
{
	ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

//Add Rate limiting
app.UseRateLimiter();


//Use exceptionHandler
//app.UseExceptionHandler();
//Add Recurrent job(hangfire)

app.UseHangfireDashboard("/jobs");

var scopFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using var scop = scopFactory.CreateScope();
var notificationService = scop.ServiceProvider.GetRequiredService<INotificationService>();

var rolManager = scop.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
var userManager = scop.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
await DefaultRoles.SeedRolesAsync(rolManager);
await DefaultUser.SeedAdminUserAsync(userManager);
RecurringJob.AddOrUpdate("SendNewPollNotification", () => notificationService.SendNewPollNotification(), Cron.Daily);//https://crontab.guru/ يوميا الساعه 12 ,ممكن تحدد كرون بالساعه واليوم والدقيقه وإلخ عن طريق عمل كرون 																													 // Configure the HTTP request pipeline.

app.Run();
