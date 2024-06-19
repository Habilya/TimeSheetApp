namespace TimeSheetApp.Api.Contracts.Data;

public class TimesheetDto
{
	public Guid Id { get; init; } = default!;
	public Guid UserId { get; init; } = default!;
	public Guid ProjectId { get; init; } = default!;
	public Guid TaskId { get; init; } = default!;
	public DateTime Date { get; init; } = default!;
	public double Hours { get; init; } = default!;
	public string Description { get; init; } = default!;
}
