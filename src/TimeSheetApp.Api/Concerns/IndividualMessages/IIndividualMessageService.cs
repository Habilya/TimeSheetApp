namespace TimeSheetApp.Api.Concerns.IndividualMessages;

public interface IIndividualMessageService
{
	Task<IEnumerable<IndividualMessage>> GetAllAsync();
	Task<IEnumerable<IndividualMessage>> SearchAsync(string searchString, DateTime? fromDate, DateTime? toDate);
}
