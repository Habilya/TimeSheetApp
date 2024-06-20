
using Microsoft.AspNetCore.Mvc;
using TimeSheetApp.Api.Services;

namespace TimeSheetApp.Api.Controllers;

[ApiController]
public class TypicodeController : ControllerBase
{
	private readonly ITypicodeService _typicodeService;

	public TypicodeController(ITypicodeService typicodeService)
	{
		_typicodeService = typicodeService;
	}

	[HttpGet("typicode/users")]
	public async Task<IActionResult> GetAll()
	{
		var users = await _typicodeService.GetAllUsersAsync();
		return Ok(users);
	}
}
