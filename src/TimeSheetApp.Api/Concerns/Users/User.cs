namespace TimeSheetApp.Api.Concerns.Users;

public class User
{
	public Guid Id { get; init; } = Guid.NewGuid();

	public string UserName { get; init; } = default!;

	public string FirstName { get; init; } = default!;

	public string LastName { get; init; } = default!;

	public string FullName => $"{FirstName} {LastName}";

	public string Email { get; init; } = default!;

	public DateTime DateOfBirth { get; init; } = default!;

	public DateTime DateCreated { get; init; } = default!;
}
