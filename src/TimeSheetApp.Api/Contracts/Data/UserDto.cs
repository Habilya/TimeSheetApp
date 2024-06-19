namespace TimeSheetApp.Api.Contracts.Data;

public class UserDto
{
	public Guid id { get; init; } = default!;

	public string username { get; init; } = default!;

	public string first_name { get; init; } = default!;

	public string last_name { get; init; } = default!;

	public string email { get; init; } = default!;

	public DateTime date_of_birth { get; init; } = default!;

	public DateTime date_created { get; init; } = default!;
}
