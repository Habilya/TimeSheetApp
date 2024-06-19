namespace TimeSheetApp.Api.Contracts.Response;

public class GetAllUsersResponse
{
	public IEnumerable<UserResponse> Users { get; init; } = Enumerable.Empty<UserResponse>();
}
