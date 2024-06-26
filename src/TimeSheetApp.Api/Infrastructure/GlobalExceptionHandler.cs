using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TimeSheetApp.Library.Logging;
using TimeSheetApp.Library.Providers;

namespace TimeSheetApp.Api.Infrastructure;

public class GlobalExceptionHandler : IExceptionHandler
{
	private readonly ILoggerAdapter<GlobalExceptionHandler> _logger;
	private readonly IGuidProvider _guidProvider;

	public GlobalExceptionHandler(ILoggerAdapter<GlobalExceptionHandler> logger, IGuidProvider guidProvider)
	{
		_logger = logger;
		_guidProvider = guidProvider;
	}

	public async ValueTask<bool> TryHandleAsync(
		HttpContext httpContext,
		Exception exception,
		CancellationToken cancellationToken)
	{
		var guid = _guidProvider.NewGuid();
		_logger.LogWarning("[Unhandled Exception] ref.: {guid}", guid);

		var problemDetailsExtensions = new Dictionary<string, object?>()
		{
			{ "ref.", guid }
		};

		var problemDetails = new ProblemDetails
		{
			Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.1",
			Title = "Internal Server Error",
			Status = (int)HttpStatusCode.InternalServerError,
			Extensions = problemDetailsExtensions
		};

		await httpContext.Response.WriteAsJsonAsync(problemDetails);

		return true;
	}
}
