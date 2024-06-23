
using Microsoft.AspNetCore.Mvc;

namespace TimeSheetApp.Api.Concerns.Typicode;

[Route("typicode")]
[ApiController]
public class TypicodeController : ControllerBase
{
	private readonly ITypicodeService _typicodeService;

	public TypicodeController(ITypicodeService typicodeService)
	{
		_typicodeService = typicodeService;
	}

	[HttpGet("users")]
	public async Task<IActionResult> GetAll()
	{
		var users = await _typicodeService.GetAllUsersAsync();
		return Ok(users);
	}
}
