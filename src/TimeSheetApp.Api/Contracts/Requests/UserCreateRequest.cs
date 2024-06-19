namespace TimeSheetApp.Api.Contracts.Requests;

public class UserCreateRequest
{
	public string UserName { get; init; } = default!;

	public string FirstName { get; init; } = default!;

	public string LastName { get; init; } = default!;

	public string Email { get; init; } = default!;

	public DateTime DateOfBirth { get; init; } = default!;
}
