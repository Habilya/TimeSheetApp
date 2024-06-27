using ErrorOr;

namespace TimeSheetApp.Api.Concerns.Users;

public static partial class Errors
{
	public static class User
	{
		public static Error UserIdAlreadyExists(Guid id) => Error.Validation(code: "User.Id.Duplicate", description: $"User with Id ({id}) already exist");

		public static Error UserNameAlreadyExists(string username) => Error.Validation(code: "User.UserName.Duplicate", description: $"User with UserName ({username}) already exist");
	}
}
