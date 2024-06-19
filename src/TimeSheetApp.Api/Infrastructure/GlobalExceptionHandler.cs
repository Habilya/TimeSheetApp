using Microsoft.AspNetCore.Diagnostics;
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
		_logger.LogWarning($"[Unhandled Exception] ref.: {guid}");

		httpContext.Response.StatusCode = 500;
		httpContext.Response.ContentType = "application/json";

		await httpContext.Response.WriteAsJsonAsync(new
		{
			StatusCode = httpContext.Response.StatusCode,
			Title = "Internal Server Error",
			Message = $"ref.: {guid}"
		});

		return true;
	}
}
