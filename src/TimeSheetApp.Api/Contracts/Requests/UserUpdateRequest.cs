namespace TimeSheetApp.Api.Contracts.Requests;

public class UserUpdateRequest : UserBaseRequest
{
	public DateTime DateCreated { get; init; } = default!;
}
