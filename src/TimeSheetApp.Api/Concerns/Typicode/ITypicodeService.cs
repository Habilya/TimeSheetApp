namespace TimeSheetApp.Api.Concerns.Typicode;

public interface ITypicodeService
{
	Task<IEnumerable<Responses.User>> GetAllUsersAsync();
}
