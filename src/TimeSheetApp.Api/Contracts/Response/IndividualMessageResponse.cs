namespace TimeSheetApp.Api.Contracts.Response
{
	public class IndividualMessageResponse
	{
		public Guid Id { get; init; } = default!;
		public int Version { get; init; } = default!;
		public string Subject { get; init; } = default!;
		public string Body { get; init; } = default!;
		public DateTime SendDate { get; init; } = default!;
	}
}
