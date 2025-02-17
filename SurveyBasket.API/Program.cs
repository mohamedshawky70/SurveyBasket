using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using SurveyBasket.API;
using SurveyBasket.API.Resources;
using System.Reflection;
var builder = WebApplication.CreateBuilder(args);

//Start:7/2/2025
//End:
builder.Services.AddDependencies(builder.Configuration);

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "v1"));
}

app.UseHttpsRedirection();
//Use CORS
app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthorization();

//app.MapIdentityApi<ApplicationUser>();

app.MapControllers();

//Use exceptionHandler
//app.UseExceptionHandler();

app.Run();
