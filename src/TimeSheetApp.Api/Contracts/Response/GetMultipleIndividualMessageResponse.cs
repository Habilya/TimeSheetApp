namespace TimeSheetApp.Api.Contracts.Response;

public class GetMultipleIndividualMessageResponse
{
	public IEnumerable<IndividualMessageResponse> IndividualMessages { get; init; } = Enumerable.Empty<IndividualMessageResponse>();
}
