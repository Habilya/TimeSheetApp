using Microsoft.AspNetCore.Mvc;
using TimeSheetApp.Api.Concerns.Base;
using TimeSheetApp.Library.Providers;

namespace TimeSheetApp.Api.Concerns.Info;

public class InfoController : ApiController
{
	private readonly IDateTimeProvider _dateTimeProvider;

	public InfoController(IDateTimeProvider dateTimeProvider)
	{
		_dateTimeProvider = dateTimeProvider;
	}

	[HttpGet]
	[Route("info")]
	public IActionResult GetInfo()
	{
		var hostInfo = new
		{
			hostName = Environment.MachineName,
			path = AppContext.BaseDirectory,
			upSince = System.Diagnostics.Process.GetCurrentProcess().StartTime,
			upTimeMinutes = (int)_dateTimeProvider.DateTimeNow.Subtract(System.Diagnostics.Process.GetCurrentProcess().StartTime).TotalMinutes,
			processorCount = Environment.ProcessorCount,
			memoryUsed = GC.GetTotalMemory(false),
			processorTimeUsedSecs = (int)_dateTimeProvider.DateTimeNow.Subtract(System.Diagnostics.Process.GetCurrentProcess().StartTime).TotalSeconds,
			serverDateTime = _dateTimeProvider.DateTimeNow
		};

		var applicationInfo = new
		{
			version = typeof(IApiMarker).Assembly.GetName().Version?.ToString(),
			buildNumber = Environment.GetEnvironmentVariable("BUILD_NUMBER")
		};

		return Ok(new { applicationInfo, hostInfo });
	}
}
