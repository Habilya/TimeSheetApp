using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Globalization;
using TimeSheetApp.Api.Contracts.Requests;
using TimeSheetApp.Api.Contracts.Response;
using TimeSheetApp.Api.Controllers;
using TimeSheetApp.Api.Domain;
using TimeSheetApp.Api.Mapping;
using TimeSheetApp.Api.Services;
using TimeSheetApp.Library.Providers;

namespace TimeSheetApp.Api.Tests.Unit;

public class UserControllerTests
{
	private readonly UserController _sut;
	private readonly IUserService _userService = Substitute.For<IUserService>();
	private readonly IDateTimeProvider _dateTimeProvider = Substitute.For<IDateTimeProvider>();


	public UserControllerTests()
	{
		_sut = new UserController(_userService, _dateTimeProvider);
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

	private static Faker<UserCreateRequest> GetUserCreateRequestGenerator()
	{
		return new Faker<UserCreateRequest>()
			.RuleFor(e => e.UserName, (f, e) => f.Internet.UserName(e.FirstName, e.LastName))
			.RuleFor(e => e.FirstName, f => f.Name.FirstName())
			.RuleFor(e => e.LastName, f => f.Name.LastName())
			.RuleFor(e => e.Email, (f, e) => f.Internet.Email(e.FirstName, e.LastName))
			.RuleFor(e => e.DateOfBirth, f => f.Date.Recent(100));
	}


	[Fact]
	public async Task GetById_ReturnOkAndObject_WhenUserExists()
	{
		// Arrange
		var user = GetUserGenerator().Generate(1).First();
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
		var users = GetUserGenerator().Generate(2);
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

		var user = GetUserCreateRequestGenerator().Generate(1).First();

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
		_userService.CreateAsync(Arg.Do<User>(x => createdUser = x)).Returns(true);


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
		_userService.CreateAsync(Arg.Any<User>()).Returns(false);

		// Act
		var result = (BadRequestResult)await _sut.Create(new UserCreateRequest());

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
