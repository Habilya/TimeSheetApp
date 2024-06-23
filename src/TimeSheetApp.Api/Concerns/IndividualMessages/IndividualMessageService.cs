using TimeSheetApp.Api.Mapping;

namespace TimeSheetApp.Api.Concerns.IndividualMessages;

public class IndividualMessageService : IIndividualMessageService
{
	private readonly IIndividualMessageRepository _individualMessageRepository;

	public IndividualMessageService(IIndividualMessageRepository individualMessageRepository)
	{
		_individualMessageRepository = individualMessageRepository;
	}


	public async Task<IEnumerable<IndividualMessage>> GetAllAsync()
	{
		var individualMessageDtos = await _individualMessageRepository.GetAllAsync();
		return individualMessageDtos.Select(x => x.ToIndividualMessage());
	}

	public async Task<IEnumerable<IndividualMessage>> SearchAsync(string searchString, DateTime? fromDate, DateTime? toDate)
	{
		var individualMessageDtos = await _individualMessageRepository.SearchAsync(searchString, fromDate, toDate);
		// Perhaps add validation for the fromDate and toDate here??
		return individualMessageDtos.Select(x => x.ToIndividualMessage());
	}
}
