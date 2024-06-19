using TimeSheetApp.Api.Database;
using TimeSheetApp.Api.Infrastructure;
using TimeSheetApp.Api.Repositories;
using TimeSheetApp.Api.Services;
using TimeSheetApp.Library.Logging;
using TimeSheetApp.Library.Providers;


var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
	Args = args,
	ContentRootPath = Directory.GetCurrentDirectory()
});

var config = builder.Configuration;


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));

builder.Services.AddSingleton<IDbConnectionFactory>(_ =>
	new DbConnectionFactory(config.GetValue<string>("Database:ConnectionString")));

builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IIndividualMessageRepository, IndividualMessageRepository>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IIndividualMessageService, IndividualMessageService>();
builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
