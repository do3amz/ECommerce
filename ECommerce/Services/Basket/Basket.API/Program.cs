
using Basket.API.Data;
using BuildingBlocks.Exceptions.Handler;

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
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
#endregion

var app = builder.Build();
#region configure pipline
app.MapCarter();
app.UseExceptionHandler(opts => { });
#endregion

app.Run();
