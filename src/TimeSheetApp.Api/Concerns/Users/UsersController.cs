using Microsoft.AspNetCore.Mvc;
using TimeSheetApp.Api.Contracts.Requests;
using TimeSheetApp.Api.Mapping;
using TimeSheetApp.Library.Providers;

namespace TimeSheetApp.Api.Concerns.Users;

[Route("users")]
[ApiController]
public class UsersController : ControllerBase
{
	private readonly IUserService _userService;
	private readonly IDateTimeProvider _dateTimeProvider;

	public UsersController(IUserService userService, IDateTimeProvider dateTimeProvider)
	{
		_userService = userService;
		_dateTimeProvider = dateTimeProvider;
	}

	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		var users = await _userService.GetAllAsync();
		var usersResponse = users.ToUsersResponse();
		return Ok(usersResponse);
	}

	[HttpGet("{id:guid}")]
	public async Task<IActionResult> Get([FromRoute] Guid id)
	{
		var user = await _userService.GetAsync(id);
		if (user is null)
		{
			return NotFound();
		}

		var userResponse = user.ToUserResponse();
		return Ok(userResponse);
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] UserCreateRequest request)
	{
		var user = request.ToUser(_dateTimeProvider.DateTimeNow);

		var created = await _userService.CreateAsync(user);
		if (!created)
		{
			return BadRequest();
		}

		var userResponse = user.ToUserResponse();
		return CreatedAtAction(nameof(Get), new { userResponse.Id }, userResponse);
	}

	[HttpPut("{id:guid}")]
	public async Task<IActionResult> Update([FromRoute] Guid id,
		[FromBody] UserUpdateRequest request)
	{
		var existingUser = await _userService.GetAsync(id);
		if (existingUser is null)
		{
			return NotFound();
		}

		var user = request.ToUser(id);
		var updated = await _userService.UpdateAsync(user);
		if (!updated)
		{
			return BadRequest();
		}

		var userResponse = user.ToUserResponse();
		return Ok(userResponse);
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> Delete([FromRoute] Guid id)
	{
		var deleted = await _userService.DeleteAsync(id);
		if (!deleted)
		{
			return NotFound();
		}

		return Ok();
	}
}
