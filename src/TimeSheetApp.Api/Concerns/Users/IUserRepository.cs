using TimeSheetApp.Api.Contracts.Data;

namespace TimeSheetApp.Api.Concerns.Users;

public interface IUserRepository
{
	Task<bool> CreateAsync(UserDto user);
	Task<bool> DeleteAsync(Guid id);
	Task<IEnumerable<UserDto>> GetAllAsync();
	Task<UserDto?> GetAsync(Guid id);
	Task<bool> UpdateAsync(UserDto user);
}
