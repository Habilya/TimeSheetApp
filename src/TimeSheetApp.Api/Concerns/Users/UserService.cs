using ErrorOr;
using FluentValidation.Results;
using TimeSheetApp.Api.Mapping;
using TimeSheetApp.Library.Logging;

namespace TimeSheetApp.Api.Concerns.Users;

public class UserService : IUserService
{
	private readonly IUserRepository _userRepository;
	private readonly ILoggerAdapter<UserService> _logger;

	public UserService(IUserRepository userRepository, ILoggerAdapter<UserService> logger)
	{
		_userRepository = userRepository;
		_logger = logger;
	}


	public async Task<User?> GetAsync(Guid id)
	{
		var userDto = await _userRepository.GetAsync(id);
		return userDto?.ToUser();
	}

	public async Task<IEnumerable<User>> GetAllAsync()
	{
		try
		{
			var userDtos = await _userRepository.GetAllAsync();
			return userDtos.Select(x => x.ToUser());
		}
		catch (Exception e)
		{
			_logger.LogError(e, "Something went wrong while retrieving all users");
			throw;
		}
	}

	public async Task<ErrorOr<User>> CreateAsync(User user)
	{
		var existingUser = await _userRepository.GetAsync(user.Id);
		if (existingUser is not null)
		{
			return Errors.User.UserIdAlreadyExists(user.Id);
		}

		var existingUserName = await _userRepository.GetByUsernameAsync(user.UserName);
		if (existingUserName is not null)
		{
			return Errors.User.UserNameAlreadyExists(user.UserName);
		}

		var userDto = user.ToUserDto();
		await _userRepository.CreateAsync(userDto);

		return user;
	}

	public async Task<bool> UpdateAsync(User user)
	{
		var userDto = user.ToUserDto();
		return await _userRepository.UpdateAsync(userDto);
	}

	public async Task<bool> DeleteAsync(Guid id)
	{
		return await _userRepository.DeleteAsync(id);
	}

	private static ValidationFailure[] GenerateValidationError(string paramName, string message)
	{
		return new[]
		{
			new ValidationFailure(paramName, message)
		};
	}
}
