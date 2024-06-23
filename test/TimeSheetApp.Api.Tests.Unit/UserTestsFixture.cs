using Bogus;
using TimeSheetApp.Api.Concerns.Users;
using TimeSheetApp.Api.Contracts.Data;
using TimeSheetApp.Api.Contracts.Requests;

namespace TimeSheetApp.Api.Tests.Unit;

public class UserTestsFixture : IDisposable
{
	public UserTestsFixture()
	{
		// The constructor is also shared between tests
		// Ususally setup code goes here
	}


	public Faker<User> GetUserGenerator()
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

	public Faker<UserDto> GetUserDtoGenerator()
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

	public Faker<UserCreateRequest> GetUserCreateRequestGenerator()
	{
		return new Faker<UserCreateRequest>()
			.RuleFor(e => e.UserName, (f, e) => f.Internet.UserName(e.FirstName, e.LastName))
			.RuleFor(e => e.FirstName, f => f.Name.FirstName())
			.RuleFor(e => e.LastName, f => f.Name.LastName())
			.RuleFor(e => e.Email, (f, e) => f.Internet.Email(e.FirstName, e.LastName))
			.RuleFor(e => e.DateOfBirth, f => f.Date.Recent(100));
	}


	public void Dispose()
	{
	}
}
