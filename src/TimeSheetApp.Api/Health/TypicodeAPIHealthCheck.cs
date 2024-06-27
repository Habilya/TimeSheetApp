using Microsoft.Extensions.Diagnostics.HealthChecks;
using TimeSheetApp.Api.Concerns.Typicode;

namespace TimeSheetApp.Api.Health;

public class TypicodeAPIHealthCheck : IHealthCheck
{
	private readonly ITypicodeService _typicodeService;

	public TypicodeAPIHealthCheck(ITypicodeService typicodeService)
	{
		_typicodeService = typicodeService;
	}

	public async Task<HealthCheckResult> CheckHealthAsync(
		HealthCheckContext context,
		CancellationToken cancellationToken = default)
	{
		try
		{
			await _typicodeService.GetAllUsersAsync();
			return HealthCheckResult.Healthy();
		}
		catch (Exception exception)
		{
			return HealthCheckResult.Unhealthy(exception: exception);
		}
	}
}
