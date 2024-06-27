using ErrorOr;

namespace TimeSheetApp.Api.Concerns.Users;

public interface IUserService
{
	Task<ErrorOr<User>> CreateAsync(User user);
	Task<bool> DeleteAsync(Guid id);
	Task<IEnumerable<User>> GetAllAsync();
	Task<User?> GetAsync(Guid id);
	Task<bool> UpdateAsync(User user);
}
