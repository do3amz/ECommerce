using BuildingBlocks.Behaviours;
using BuildingBlocks.Exceptions.Handler;
using Catalog.API.Data;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
#region add services to container
builder.Services.AddMediatR(config => { config.RegisterServicesFromAssembly(typeof(Program).Assembly);
	config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
	config.AddOpenBehavior(typeof(LoggingBehaviour<,>));

});
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddCarter();

builder.Services.AddMarten(opt => {
	opt.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();
if(builder.Environment.IsDevelopment())
{
	builder.Services.InitializeMartenWith<CatalogInitialData>();
}
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddHealthChecks().AddNpgSql(builder.Configuration.GetConnectionString("Database")!);
#endregion
var app = builder.Build();
#region configure http pipeline
app.MapCarter();
app.UseExceptionHandler(options => { });
app.UseHealthChecks("/health",
	new HealthCheckOptions
	{
		ResponseWriter=UIResponseWriter.WriteHealthCheckUIResponse
	});
#endregion


app.Run();
