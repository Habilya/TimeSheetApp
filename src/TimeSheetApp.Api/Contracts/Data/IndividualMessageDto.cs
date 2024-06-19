namespace TimeSheetApp.Api.Contracts.Data;

public class IndividualMessageDto
{
	public Guid Id { get; init; } = default!;
	public int Version { get; init; } = default!;
	public DateTime CreationDate { get; init; } = default!;
	public Guid CreatedBy { get; init; } = default!;
	public DateTime? LastUpdateDate { get; init; } = default!;
	public Guid? LastUpdatedBy { get; init; } = default!;
	public DateTime? DeletionDate { get; init; } = default!;
	public Guid? DeletedBy { get; init; } = default!;
	public DateTime? ArchivalDate { get; init; } = default!;
	public Guid? ArchivedBy { get; init; } = default!;
	public string Subject { get; init; } = default!;
	public string Body { get; init; } = default!;
	public DateTime SendDate { get; init; } = default!;
	public bool IsTask { get; init; } = default!;
	public DateTime? StartDate { get; init; } = default!;
	public DateTime? DueDate { get; init; } = default!;
	public bool IsDraft { get; init; } = default!;
	public bool? IsGroupTask { get; init; } = default!;
	public Guid? DocumentPatientId { get; init; } = default!;
	public string FileName { get; init; } = default!;
	public Guid? TypeTaskLookupId { get; init; } = default!;
	public Guid? PriorityLookupId { get; init; } = default!;
	public Guid FromContactId { get; init; } = default!;
}
