using TimeSheetApp.Api.Domain;

namespace TimeSheetApp.Api.Services;

public interface IIndividualMessageService
{
	Task<IEnumerable<IndividualMessage>> GetAllAsync();
	Task<IEnumerable<IndividualMessage>> SearchAsync(string searchString, DateTime? fromDate, DateTime? toDate);
}
