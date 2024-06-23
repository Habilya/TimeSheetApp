using Microsoft.Net.Http.Headers;
using Serilog;
using TimeSheetApp.Api.Concerns.IndividualMessages;
using TimeSheetApp.Api.Concerns.Typicode;
using TimeSheetApp.Api.Concerns.Users;
using TimeSheetApp.Api.Database;
using TimeSheetApp.Api.Infrastructure;
using TimeSheetApp.Api.Options;
using TimeSheetApp.Library.Logging;
using TimeSheetApp.Library.Providers;


var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
	Args = args,
	ContentRootPath = AppContext.BaseDirectory
});

Environment.CurrentDirectory = AppContext.BaseDirectory;

// Add services to the container.
var config = builder.Configuration;
#region Serilog configuration
var logger = new LoggerConfiguration()
	.ReadFrom.Configuration(config)
	.Enrich.FromLogContext()
	.CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
#endregion

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region DB Configuration
var dbConnectionOptionsSection = config.GetSection(nameof(DbConnectionOptions));
builder.Services.Configure<DbConnectionOptions>(dbConnectionOptionsSection);
builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
#endregion

#region API Typicode
builder.Services.AddSingleton<ITypicodeService, TypicodeService>();
builder.Services.AddHttpClient("TypicodeApi", httpClient =>
{
	httpClient.BaseAddress = new Uri(config.GetValue<string>("TypicodeApiConfigurationOptions:ApiBaseUrl")!);
	httpClient.DefaultRequestHeaders.Add(
		HeaderNames.Accept, "application/json");
});
#endregion

#region Repositories
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IIndividualMessageRepository, IndividualMessageRepository>();
#endregion

#region Services
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IIndividualMessageService, IndividualMessageService>();
#endregion

#region Abstractions (To make codebase testable)
builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddSingleton<IGuidProvider, GuidProvider>();
builder.Services.AddTransient(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
#endregion

#region Global Exception Handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
#endregion

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
