using TimeSheetApp.Api.Domain;

namespace TimeSheetApp.Api.Services;

public interface IUserService
{
	Task<bool> CreateAsync(User user);
	Task<bool> DeleteAsync(Guid id);
	Task<IEnumerable<User>> GetAllAsync();
	Task<User?> GetAsync(Guid id);
	Task<bool> UpdateAsync(User user);
}
