namespace TimeSheetApp.Api.Concerns.TimeSheets;

public class TimeSheet
{
	public Guid Id { get; init; } = Guid.NewGuid();

	public Guid UserId { get; init; } = default!;

	public Guid ProjectId { get; init; } = default!;

	public Guid TaskId { get; init; } = default!;

	public DateTime Date { get; init; } = default!;

	public double Hours { get; init; } = default!;

	public string Description { get; init; } = default!;
}
