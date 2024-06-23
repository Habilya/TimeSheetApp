using FluentValidation.Results;

namespace TimeSheetApp.Api.Validation;

public record ValidationFailed(IEnumerable<ValidationFailure> Errors)
{
	public ValidationFailed(ValidationFailure error) : this(new[] { error })
	{
	}
}
