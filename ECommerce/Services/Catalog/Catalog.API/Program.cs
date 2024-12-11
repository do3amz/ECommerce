using BuildingBlocks.Behaviours;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
#region add services to container
builder.Services.AddMediatR(config => { config.RegisterServicesFromAssembly(typeof(Program).Assembly);
	config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
  });
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddCarter();

builder.Services.AddMarten(opt => {
	opt.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

#endregion
var app = builder.Build();
#region configure http pipeline
app.MapCarter();
app.UseExceptionHandler(exceptionHandlerApp =>
{
	exceptionHandlerApp.Run(async context =>
	{
		var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
		if(exception==null)
		{
			return;
		}
		var problemDetails = new ProblemDetails
		{
			Title = exception.Message,
		    Status=StatusCodes.Status500InternalServerError,
			Detail=exception.StackTrace
		};
		var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
		logger.LogError(exception, exception.Message);
		context.Response.StatusCode = StatusCodes.Status500InternalServerError;
		context.Response.ContentType = "application/problem+json";
		await context.Response.WriteAsJsonAsync(problemDetails);
	});
});

#endregion


app.Run();
