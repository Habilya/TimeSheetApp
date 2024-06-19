using TimeSheetApp.Library.Providers;

namespace TimeSheetApp.Api.Domain;

public class TimeSheet
{
	private readonly IDateTimeProvider _dateTimeProvider;

	public TimeSheet(IDateTimeProvider dateTimeProvider)
	{
		_dateTimeProvider = dateTimeProvider;
		Date = _dateTimeProvider.DateTimeNow;
	}

	public Guid Id { get; init; } = Guid.NewGuid();

	public Guid UserId { get; init; } = default!;

	public Guid ProjectId { get; init; } = default!;

	public Guid TaskId { get; init; } = default!;

	// Moved the dependency injetion initializer into constructor
	public DateTime Date { get; init; } = default!;

	public double Hours { get; init; } = default!;

	public string Description { get; init; } = default!;
}
