using TimeSheetApp.Api.Typicode.Responses;

namespace TimeSheetApp.Api.Services;

public interface ITypicodeService
{
	Task<IEnumerable<User>> GetAllUsersAsync();
}
