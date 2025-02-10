using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using SurveyBasket.API;
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

app.UseAuthorization();

app.MapControllers();

app.Run();
