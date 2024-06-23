using TimeSheetApp.Api.Contracts.Data;

namespace TimeSheetApp.Api.Concerns.IndividualMessages
{
	public interface IIndividualMessageRepository
	{
		Task<IEnumerable<IndividualMessageDto>> GetAllAsync();
		Task<IEnumerable<IndividualMessageDto>> SearchAsync(string searchString, DateTime? fromDate, DateTime? toDate);
	}
}
