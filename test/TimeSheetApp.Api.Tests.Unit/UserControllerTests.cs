using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Globalization;
using TimeSheetApp.Api.Concerns.Errors;
using TimeSheetApp.Api.Concerns.Users;
using TimeSheetApp.Api.Contracts.Requests;
using TimeSheetApp.Api.Contracts.Response;
using TimeSheetApp.Api.Mapping;
using TimeSheetApp.Library.Providers;

namespace TimeSheetApp.Api.Tests.Unit;

public class UserControllerTests : IClassFixture<UserTestsFixture>
{
	private readonly UsersController _sut;
	private readonly UserTestsFixture _fixture;
	private readonly IUserService _userService = Substitute.For<IUserService>();
	private readonly IDateTimeProvider _dateTimeProvider = Substitute.For<IDateTimeProvider>();

	private readonly HttpContext _httpContext = Substitute.For<HttpContext>();


	public UserControllerTests(UserTestsFixture fixture)
	{
		var services = new ServiceCollection();
		services.AddSingleton<ProblemDetailsFactory, TimeSheetAPIProblemDetailsFactory>();

		_sut = new UsersController(_userService, _dateTimeProvider);
		_sut.ControllerContext = new ControllerContext
		{
			HttpContext = _httpContext
		};
		_fixture = fixture;
	}


	[Fact]
	public async Task GetById_ReturnOkAndObject_WhenUserExists()
	{
		// Arrange
		var user = _fixture.GetUserGenerator().Generate(1).First();
		_userService.GetAsync(user.Id).Returns(user);

		// Act
		var result = (OkObjectResult)await _sut.Get(user.Id);

		// Assert
		result.StatusCode.Should().Be(200);
		result.Value.Should().BeEquivalentTo(user);
	}

	[Fact]
	public async Task GetById_ReturnNotFound_WhenUserDoesntExists()
	{
		// Arrange
		_userService.GetAsync(Arg.Any<Guid>()).ReturnsNull();

		// Act
		var result = (NotFoundResult)await _sut.Get(Guid.NewGuid());

		// Assert
		result.StatusCode.Should().Be(404);
	}

	[Fact]
	public async Task GetAll_ShouldReturnEmptyList_WhenNoUserExists()
	{
		// Arrange
		_userService.GetAllAsync().Returns(Enumerable.Empty<User>());

		// Act
		var result = (OkObjectResult)await _sut.GetAll();

		// Assert
		result.StatusCode.Should().Be(200);
		result.Value.As<GetAllUsersResponse>().Users.Should().BeEmpty();
	}

	[Fact]
	public async Task GetAll_ReturnOkAndObject_WhenUserExists()
	{
		// Arrange
		var users = _fixture.GetUserGenerator().Generate(2);
		_userService.GetAllAsync().Returns(users);

		// Act
		var result = (OkObjectResult)await _sut.GetAll();

		// Assert
		result.StatusCode.Should().Be(200);
		result.Value.As<GetAllUsersResponse>().Users.Should().BeEquivalentTo(users.Select(x => x.ToUserResponse()));
	}

	[Fact]
	public async Task Create_ShouldCreateUser_WhenCreateUserRequestIsValid()
	{
		// Arrange
		var expectedCreatedDate = new DateTime(2020, 1, 1, 20, 0, 0);
		_dateTimeProvider.DateTimeNow.Returns(expectedCreatedDate);

		var user = _fixture.GetUserCreateRequestGenerator().Generate(1).First();

		var createdUser = new User
		{
			Id = Guid.NewGuid(),
			UserName = user.UserName,
			FirstName = user.FirstName,
			LastName = user.LastName,
			Email = user.Email,
			DateOfBirth = user.DateOfBirth,
			DateCreated = DateTime.ParseExact("2020-01-01_20:00:00", "yyyy-MM-dd_HH:mm:ss", CultureInfo.InvariantCulture)
		};
		_userService.CreateAsync(Arg.Do<User>(x => createdUser = x)).Returns(createdUser);


		// Act
		var result = (CreatedAtActionResult)await _sut.Create(user);
		var userResponse = createdUser.ToUserResponse();

		// Assert
		result.StatusCode.Should().Be(201);
		result.Value.As<UserResponse>().Should().BeEquivalentTo(userResponse);
		result.RouteValues!["id"].Should().Be(userResponse.Id);
	}

	[Fact]
	public async Task Create_ShouldReturnBadRequest_WhenCreateUserRequestIsInvalid()
	{
		// Arrange
		var guid = Guid.NewGuid();
		_userService.CreateAsync(Arg.Any<User>()).Returns(Errors.User.UserIdAlreadyExists(guid));

		// Act
		var result = (ObjectResult)await _sut.Create(new UserCreateRequest());

		// Assert
		result.StatusCode.Should().Be(400);
	}

	[Fact]
	public async Task DeleteById_ReturnOk_WhenUserWasDeleted()
	{
		// Arrange
		_userService.DeleteAsync(Arg.Any<Guid>()).Returns(true);

		// Act
		var result = (OkResult)await _sut.Delete(Guid.NewGuid());

		// Assert
		result.StatusCode.Should().Be(200);
	}

	[Fact]
	public async Task DeleteById_ReturnNotFound_WhenUserDoesntExists()
	{
		// Arrange
		_userService.DeleteAsync(Arg.Any<Guid>()).Returns(false);

		// Act
		var result = (NotFoundResult)await _sut.Delete(Guid.NewGuid());

		// Assert
		result.StatusCode.Should().Be(404);
	}
}
