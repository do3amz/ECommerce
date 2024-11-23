var builder = WebApplication.CreateBuilder(args);
#region add services to container
builder.Services.AddCarter();
builder.Services.AddMediatR(config => { config.RegisterServicesFromAssembly(typeof(Program).Assembly); });
builder.Services.AddMarten(opt => {
	opt.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

#endregion
var app = builder.Build();
#region configure http pipeline
app.MapCarter();


#endregion


app.Run();
