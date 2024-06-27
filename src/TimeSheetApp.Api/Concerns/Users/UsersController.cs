using Microsoft.AspNetCore.Mvc;
using TimeSheetApp.Api.Concerns.Base;
using TimeSheetApp.Api.Contracts.Requests;
using TimeSheetApp.Api.Mapping;
using TimeSheetApp.Library.Providers;

namespace TimeSheetApp.Api.Concerns.Users;

[Route("users")]
public class UsersController : ApiController
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
	public async Task<IActionResult> Get(Guid id)
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
	public async Task<IActionResult> Create(UserCreateRequest request)
	{
		var user = request.ToUser(_dateTimeProvider.DateTimeNow);

		var created = await _userService.CreateAsync(user);

		return created.Match(
			result =>
			{
				var userResponse = user.ToUserResponse();
				return CreatedAtAction(nameof(Get), new { userResponse.Id }, userResponse);
			},
			errors => Problem(errors)
		);
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
	public async Task<IActionResult> Delete(Guid id)
	{
		var deleted = await _userService.DeleteAsync(id);
		if (!deleted)
		{
			return NotFound();
		}

		return Ok();
	}
}
