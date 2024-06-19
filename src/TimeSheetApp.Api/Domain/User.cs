namespace TimeSheetApp.Api.Domain;

public class User
{
	public Guid Id { get; init; } = Guid.NewGuid();

	public string UserName { get; init; } = default!;

	public string FirstName { get; init; } = default!;

	public string LastName { get; init; } = default!;

	public string FullName => $"{FirstName} {LastName}";

	public string Email { get; init; } = default!;

	// For validation I need to validate against current date need to inject IDateTimeProvider
	public DateTime DateOfBirth { get; init; } = default!;

	public DateTime DateCreated { get; init; } = default!;
}

/*
 private static readonly Regex EmailAddressValidationRegex = new(
			"^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,6}$",
			RegexOptions.Compiled | RegexOptions.IgnoreCase
		);


		// Accept Name with white space, hyphen, dot, apostrophe, small letters, large letters, accented letters
		private static readonly Regex NameValidationRegex = new(
			"^[A-Za-zÀ-ÖØ-öø-ÿ ,.'-]+$",
			RegexOptions.Compiled
		);
 
 private static readonly Regex UserNameValidationRegex = new(
			"^([a-z0-9_-]+){4,38}$",
			RegexOptions.Compiled | RegexOptions.IgnoreCase
		);
 */
