using FluentValidation;
using System.Text.RegularExpressions;
using TimeSheetApp.Api.Contracts.Requests;

namespace TimeSheetApp.Api.Concerns.Users;

public class UserBaseValidator<T> : AbstractValidator<T> where T : UserBaseRequest
{
	public UserBaseValidator()
	{
		RuleFor(m => m.UserName)
			.NotEmpty()
			.IsValidUserName();

		RuleFor(m => m.FirstName)
			.NotEmpty()
			.IsValidBasicName()
			.WithMessage("First Name invalid");

		RuleFor(m => m.LastName)
			.NotEmpty()
			.IsValidBasicName()
			.WithMessage("Last Name invalid");

		RuleFor(m => m.Email)
			.NotEmpty()
			.IsValidEmail();

		RuleFor(m => m.DateOfBirth)
			.NotEmpty();
	}
}

public class UserCreateValidator : UserBaseValidator<UserCreateRequest> { }

public class UserUpdateValidator : UserBaseValidator<UserUpdateRequest>
{
	public UserUpdateValidator()
	{
		RuleFor(m => m.DateCreated)
			.NotEmpty();
	}
}


public static class UserNameValidator
{
	private static readonly Regex UserNameValidationRegex = new(
		"^([a-z0-9_-]+){4,38}$",
		RegexOptions.Compiled | RegexOptions.IgnoreCase
	);

	public static IRuleBuilderOptions<T, string> IsValidUserName<T>(this IRuleBuilder<T, string> ruleBuilder)
	{
		return ruleBuilder.Must(IsUserName);
	}
	private static bool IsUserName(string value)
	{
		return UserNameValidationRegex.IsMatch(value);
	}
}

public static class UserBasicNameValidator
{
	private static readonly Regex NameValidationRegex = new(
		"^[A-Za-zÀ-ÖØ-öø-ÿ ,.'-]+$",
		RegexOptions.Compiled | RegexOptions.IgnoreCase
	);

	public static IRuleBuilderOptions<T, string> IsValidBasicName<T>(this IRuleBuilder<T, string> ruleBuilder)
	{
		return ruleBuilder.Must(IsBasicName);
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
		return ruleBuilder.Must(IsEmail).WithMessage("This is not a valid e-mail address.");
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
