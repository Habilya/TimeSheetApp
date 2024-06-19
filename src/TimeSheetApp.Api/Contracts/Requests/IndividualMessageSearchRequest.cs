namespace TimeSheetApp.Api.Contracts.Requests;

public class IndividualMessageSearchRequest
{
	public string SearchString { get; init; } = default!;
	public DateTime? FromDate { get; init; }
	public DateTime? ToDate { get; init; }
}
