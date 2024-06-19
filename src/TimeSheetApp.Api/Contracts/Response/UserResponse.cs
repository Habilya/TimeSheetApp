namespace TimeSheetApp.Api.Contracts.Response;

public class UserResponse
{
	public Guid Id { get; init; }

	public string UserName { get; init; } = default!;

	public string FirstName { get; init; } = default!;

	public string LastName { get; init; } = default!;

	public string FullName { get; init; } = default!;

	public string Email { get; init; } = default!;

	public DateTime DateOfBirth { get; init; } = default!;

	public DateTime DateCreated { get; init; } = default!;
}
