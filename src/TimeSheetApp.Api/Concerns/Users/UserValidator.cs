using FluentValidation;
using System.Text.RegularExpressions;
using TimeSheetApp.Api.Contracts.Requests;
using TimeSheetApp.Library.Providers;

namespace TimeSheetApp.Api.Concerns.Users;

public class UserBaseValidator<T> : AbstractValidator<T> where T : UserBaseRequest
{
	public UserBaseValidator(IDateTimeProvider dateTimeProvider)
	{
		RuleFor(m => m.UserName)
			.NotEmpty()
			.IsValidUserName();

		RuleFor(m => m.FirstName)
			.NotEmpty()
			.IsValidBasicName();

		RuleFor(m => m.LastName)
			.NotEmpty()
			.IsValidBasicName();

		RuleFor(m => m.Email)
			.NotEmpty()
			.IsValidEmail();

		RuleFor(m => m.DateOfBirth)
			.NotEmpty()
			.Must(x => x < dateTimeProvider.DateTimeNow)
			.WithMessage("{PropertyName} is invalid, must be before current DateTime " + dateTimeProvider.DateTimeNow.ToString());
	}
}

public class UserCreateValidator : UserBaseValidator<UserCreateRequest>
{
	public UserCreateValidator(IDateTimeProvider dateTimeProvider) : base(dateTimeProvider) { }
}

public class UserUpdateValidator : UserBaseValidator<UserUpdateRequest>
{
	public UserUpdateValidator(IDateTimeProvider dateTimeProvider) : base(dateTimeProvider)
	{
		RuleFor(m => m.DateCreated)
			.NotEmpty();
	}
}


public static class UserNameValidator
{
	private static readonly Regex UserNameValidationRegex = new(
		"^[a-zA-Z0-9_-]{4,50}$",
		RegexOptions.Compiled | RegexOptions.IgnoreCase
	);

	public static IRuleBuilderOptions<T, string> IsValidUserName<T>(this IRuleBuilder<T, string> ruleBuilder)
	{
		return ruleBuilder
			.Must(IsUserName)
			.WithMessage("{PropertyName} is invalid, must be from 4 to 50 characters, " +
			"allowed characters are (a-z) lower and upper case, uderscores (_), " +
			"dashes (-), digits (0-9)");
	}

	private static bool IsUserName(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			return false;
		}

		return UserNameValidationRegex.IsMatch(value);
	}
}

public static class UserBasicNameValidator
{
	private static readonly Regex NameValidationRegex = new(
		"^[a-zA-ZÀ-ÖØ-öø-ÿ ,.'-]{1,50}$",
		RegexOptions.Compiled | RegexOptions.IgnoreCase
	);

	public static IRuleBuilderOptions<T, string> IsValidBasicName<T>(this IRuleBuilder<T, string> ruleBuilder)
	{
		return ruleBuilder
			.Must(IsBasicName)
			.WithMessage("{PropertyName} is invalid, must be from 1 to 50 characters, " +
			"allowed characters are (a-z) lower and upper case + Accented characters, spaces, " +
			"special characters (,.'-)"); ;
	}

	private static bool IsBasicName(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			return false;
		}

		return NameValidationRegex.IsMatch(value);
	}
}

public static class UserEmailValidator
{
	private static readonly Regex EmailAddressValidationRegex = new(
		@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
		RegexOptions.Compiled | RegexOptions.IgnoreCase
	);

	public static IRuleBuilderOptions<T, string> IsValidEmail<T>(this IRuleBuilder<T, string> ruleBuilder)
	{
		return ruleBuilder
			.Must(IsEmail)
			.WithMessage("{PropertyName} is invalid, not a valid e-mail address.");
	}

	private static bool IsEmail(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			return false;
		}

		return EmailAddressValidationRegex.IsMatch(value);
	}
}
