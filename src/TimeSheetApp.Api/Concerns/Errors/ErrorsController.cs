using Microsoft.AspNetCore.Mvc;
using TimeSheetApp.Library.Logging;
using TimeSheetApp.Library.Providers;

namespace TimeSheetApp.Api.Concerns.Errors;

[ApiController]
public class ErrorsController : ControllerBase
{
	private readonly ILoggerAdapter<ErrorsController> _logger;
	private readonly IGuidProvider _guidProvider;

	public ErrorsController(ILoggerAdapter<ErrorsController> logger, IGuidProvider guidProvider)
	{
		_logger = logger;
		_guidProvider = guidProvider;
	}

	[HttpGet]
	[Route("/error")]
	public IActionResult Error()
	{
		// If you want, you can access the Exception from HttpContext
		/*
		var exception = HttpContext.Features
			.Get<IExceptionHandlerFeature>()?.Error;
		*/

		var refNumber = _guidProvider.NewGuid();
		_logger.LogWarning("detail: {refNumber}", refNumber);

		return Problem(detail: refNumber.ToString());
	}
}
