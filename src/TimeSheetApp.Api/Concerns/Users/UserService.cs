using FluentValidation;
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

	public async Task<bool> CreateAsync(User user)
	{
		var existingUser = await _userRepository.GetAsync(user.Id);
		if (existingUser is not null)
		{
			var message = $"A user with id {user.Id} already exists";
			throw new ValidationException(message, GenerateValidationError(nameof(User), message));
		}
		// Move it to a different location
		//user.DateCreated = _dateTimeProvider.DateTimeNow;
		var userDto = user.ToUserDto();
		return await _userRepository.CreateAsync(userDto);
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
