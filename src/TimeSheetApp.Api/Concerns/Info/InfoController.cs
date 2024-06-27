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
		var version = typeof(IApiMarker).Assembly.GetName().Version?.ToString();
		var buildNumber = Environment.GetEnvironmentVariable("BUILD_NUMBER");
		var workstation = Environment.MachineName;
		var serverDateTime = _dateTimeProvider.DateTimeNow;

		var info = new
		{
			Version = version,
			BuildNumber = buildNumber,
			Workstation = workstation,
			ServerDateTime = serverDateTime
		};

		return Ok(info);
	}
}
