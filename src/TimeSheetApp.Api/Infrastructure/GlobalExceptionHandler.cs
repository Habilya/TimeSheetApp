using Microsoft.AspNetCore.Diagnostics;
using TimeSheetApp.Library.Logging;

namespace TimeSheetApp.Api.Infrastructure;

public class GlobalExceptionHandler : IExceptionHandler
{
	private readonly ILoggerAdapter<GlobalExceptionHandler> _logger;

	public GlobalExceptionHandler(ILoggerAdapter<GlobalExceptionHandler> logger)
	{
		_logger = logger;
	}

	public async ValueTask<bool> TryHandleAsync(
		HttpContext httpContext,
		Exception exception,
		CancellationToken cancellationToken)
	{
		var guid = Guid.NewGuid();
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
