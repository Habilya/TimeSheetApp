﻿using FluentValidation;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Net.Http.Headers;
using Serilog;
using TimeSheetApp.Api;
using TimeSheetApp.Api.Concerns.IndividualMessages;
using TimeSheetApp.Api.Concerns.Typicode;
using TimeSheetApp.Api.Concerns.Users;
using TimeSheetApp.Api.Database;
using TimeSheetApp.Api.Health;
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

builder.Services.AddHealthChecks()
	.AddCheck<DatabaseHealthCheck>("Database")
	.AddCheck<TypicodeAPIHealthCheck>("TypicodeAPI");

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<IApiMarker>(ServiceLifetime.Singleton);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();

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

#region User
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IUserService, UserService>();
#endregion

#region IndividualMessage
builder.Services.AddSingleton<IIndividualMessageRepository, IndividualMessageRepository>();
builder.Services.AddSingleton<IIndividualMessageService, IndividualMessageService>();
#endregion

#region Abstractions (To make codebase testable)
builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddSingleton<IGuidProvider, GuidProvider>();
builder.Services.AddTransient(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Authentication
// Authorization

app.UseExceptionHandler("/error");

app.MapHealthChecks("/_health", new HealthCheckOptions
{
	ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
})/*.RequireAuthorization()*/;

app.UseAuthorization();

app.MapControllers();

app.Run();
