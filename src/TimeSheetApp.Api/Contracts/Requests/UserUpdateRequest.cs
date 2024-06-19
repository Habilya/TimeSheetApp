using Microsoft.AspNetCore.Mvc;

namespace TimeSheetApp.Api.Contracts.Requests;

public class UserUpdateRequest
{
	[FromRoute(Name = "id")]
	public Guid Id { get; init; }

	[FromBody]
	public string UserName { get; init; } = default!;

	[FromBody]
	public string FirstName { get; init; } = default!;

	[FromBody]
	public string LastName { get; init; } = default!;

	[FromBody]
	public string Email { get; init; } = default!;

	[FromBody]
	public DateTime DateOfBirth { get; init; } = default!;

	[FromBody]
	public DateTime DateCreated { get; init; } = default!;
}
