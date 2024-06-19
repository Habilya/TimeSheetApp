using Bogus;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using TimeSheetApp.Api.Contracts.Data;
using TimeSheetApp.Api.Domain;
using TimeSheetApp.Api.Mapping;
using TimeSheetApp.Api.Repositories;
using TimeSheetApp.Api.Services;
using TimeSheetApp.Library.Logging;

namespace TimeSheetApp.Api.Tests.Unit;

public class UserServiceTests
{
	private readonly UserService _sut;
	private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
	private readonly ILoggerAdapter<UserService> _logger = Substitute.For<ILoggerAdapter<UserService>>();

	public UserServiceTests()
	{
		_sut = new UserService(_userRepository, _logger);
	}

	private static Faker<User> GetUserGenerator()
	{
		return new Faker<User>()
			.RuleFor(e => e.Id, _ => Guid.NewGuid())
			.RuleFor(e => e.UserName, (f, e) => f.Internet.UserName(e.FirstName, e.LastName))
			.RuleFor(e => e.FirstName, f => f.Name.FirstName())
			.RuleFor(e => e.LastName, f => f.Name.LastName())
			.RuleFor(e => e.Email, (f, e) => f.Internet.Email(e.FirstName, e.LastName))
			.RuleFor(e => e.DateOfBirth, f => f.Date.Recent(100))
			.RuleFor(e => e.DateCreated, f => f.Date.Recent(100));
	}

	private static Faker<UserDto> GetUserDtoGenerator()
	{
		return new Faker<UserDto>()
			.RuleFor(e => e.id, _ => Guid.NewGuid())
			.RuleFor(e => e.username, (f, e) => f.Internet.UserName(e.first_name, e.last_name))
			.RuleFor(e => e.first_name, f => f.Name.FirstName())
			.RuleFor(e => e.last_name, f => f.Name.LastName())
			.RuleFor(e => e.email, (f, e) => f.Internet.Email(e.first_name, e.last_name))
			.RuleFor(e => e.date_of_birth, f => f.Date.Recent(100))
			.RuleFor(e => e.date_created, f => f.Date.Recent(100));
	}

	[Fact]
	public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoUsersExist()
	{
		// Arrange
		_userRepository.GetAllAsync().Returns(Enumerable.Empty<UserDto>());

		// Act
		var result = await _sut.GetAllAsync();

		// Assert
		result.Should().BeEmpty();
	}

	[Fact]
	public async Task GetAllAsync_ShouldReturnUsers_WhenSomeUsersExist()
	{
		// Arrange
		var expected = GetUserDtoGenerator().Generate(1);
		_userRepository.GetAllAsync().Returns(expected);

		// Act
		var result = await _sut.GetAllAsync();

		// Assert
		result.Should().BeEquivalentTo(expected.Select(s => s.ToUser()));
	}

	[Fact]
	public async Task GetAllAsync_ShouldThrowException_WhenExceptionIsThrown()
	{
		// Arrange
		var someException = new Exception("Somtehing went wrong");
		_userRepository.GetAllAsync().Throws(someException);

		// Act
		var requestAction = async () => await _sut.GetAllAsync();

		// Assert
		await requestAction.Should()
			.ThrowAsync<Exception>()
			.WithMessage("Somtehing went wrong");

		_logger.Received(1)
			.LogError(Arg.Is(someException), Arg.Is("Something went wrong while retrieving all users"));
	}

	[Fact]
	public async Task GetByIdAsync_ShouldReturnNull_WhenNoUsersExist()
	{
		// Arrange
		_userRepository.GetAsync(Arg.Any<Guid>()).ReturnsNull();

		// Act
		var result = await _sut.GetAsync(Guid.NewGuid());

		// Assert
		result.Should().BeNull();
	}

	[Fact]
	public async Task GetByIdAsync_ShouldReturnUser_WhenUserExist()
	{
		// Arrange
		var existingUser = GetUserDtoGenerator().Generate(1).First();
		_userRepository.GetAsync(existingUser.id).Returns(existingUser);

		// Act
		var result = await _sut.GetAsync(existingUser.id);

		// Assert
		result.Should().BeEquivalentTo(existingUser.ToUser());
	}

	[Fact]
	public async Task CreateAsync_ShouldCreateUser_WhenDetailsAreValid()
	{
		// Arrange
		var user = GetUserGenerator().Generate(1).First();
		var userDto = user.ToUserDto();
		_userRepository.CreateAsync(Arg.Do<UserDto>(x => userDto = x)).Returns(true);

		// Act
		var result = await _sut.CreateAsync(user);

		// Assert
		result.Should().BeTrue();
	}

	[Fact]
	public async Task DeleteByIdAsync_ShouldDeleteUser_WhenUserExist()
	{
		// Arrange
		var userId = Guid.NewGuid();
		_userRepository.DeleteAsync(userId).Returns(true);

		// Act
		var result = await _sut.DeleteAsync(userId);

		// Assert
		result.Should().BeTrue();
	}

	[Fact]
	public async Task DeleteByIdAsync_ShouldNotDeleteUser_WhenUserDoesntExist()
	{
		// Arrange
		var userId = Guid.NewGuid();
		_userRepository.DeleteAsync(userId).Returns(false);

		// Act
		var result = await _sut.DeleteAsync(userId);

		// Assert
		result.Should().BeFalse();
	}
}
