namespace TimeSheetApp.Api.Concerns.Typicode.Responses;

public class Address
{
	public string Street { get; init; } = default!;
	public string Suite { get; init; } = default!;
	public string City { get; init; } = default!;
	public string Zipcode { get; init; } = default!;
	public Geo Geo { get; init; } = default!;
}
