using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using TimeSheetApp.Api.Concerns.Users;
using TimeSheetApp.Api.Contracts.Data;
using TimeSheetApp.Api.Mapping;
using TimeSheetApp.Library.Logging;

namespace TimeSheetApp.Api.Tests.Unit;

public class UserServiceTests : IClassFixture<UserTestsFixture>
{
	private readonly UserService _sut;
	private readonly UserTestsFixture _fixture;
	private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
	private readonly ILoggerAdapter<UserService> _logger = Substitute.For<ILoggerAdapter<UserService>>();

	public UserServiceTests(UserTestsFixture fixture)
	{
		_sut = new UserService(_userRepository, _logger);
		_fixture = fixture;
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
		var expected = _fixture.GetUserDtoGenerator().Generate(1);
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
		var existingUser = _fixture.GetUserDtoGenerator().Generate(1).First();
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
		var user = _fixture.GetUserGenerator().Generate(1).First();
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
