
using Basket.API.Data;
using BuildingBlocks.Exceptions.Handler;
using Discount.grpc.Protos;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
var assembly=typeof(Program).Assembly;
#region register services
builder.Services.AddCarter();
builder.Services.AddMediatR(config => {
	config.RegisterServicesFromAssemblies(assembly);
	config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
	config.AddOpenBehavior(typeof(LoggingBehaviour<,>));

});
builder.Services.AddMarten(opts=>
{
	opts.Connection(builder.Configuration.GetConnectionString("Database")!);
	opts.Schema.For<ShoppingCart>().Identity(x=>x.UserName);
}
).UseLightweightSessions();
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();
builder.Services.AddStackExchangeRedisCache(cache =>
{
	cache.Configuration = builder.Configuration.GetConnectionString("Redis");
	//cache.InstanceName = "Basket";
});
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
{
	options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
});
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddHealthChecks().AddNpgSql(builder.Configuration.GetConnectionString("Database")!).AddRedis(builder.Configuration.GetConnectionString("Redis")!);
#endregion

var app = builder.Build();
#region configure pipline
app.MapCarter();
app.UseExceptionHandler(opts => { });
app.UseHealthChecks("/health", new HealthCheckOptions
{
	 ResponseWriter=UIResponseWriter.WriteHealthCheckUIResponse
});
#endregion

app.Run();
