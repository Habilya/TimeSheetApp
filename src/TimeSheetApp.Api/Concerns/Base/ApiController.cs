using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TimeSheetApp.Api.Concerns.Base;

[ApiController]
public class ApiController : ControllerBase
{
	protected IActionResult Problem(List<Error> errors)
	{
		errors ??= new List<Error>();

		if (errors.Count is 0)
		{
			// maybe log this
			return Problem();
		}

		if (errors.All(error => error.Type == ErrorType.Validation))
		{
			return ValidationProblem(errors);
		}

		return CustomProblem(errors);
	}

	private IActionResult ValidationProblem(List<Error> errors)
	{
		var modelStateDictionary = new ModelStateDictionary();

		foreach (var error in errors)
		{
			modelStateDictionary.AddModelError(
				error.Code,
				error.Description);
		}

		return ValidationProblem(modelStateDictionary);
	}

	private IActionResult CustomProblem(List<Error> errors)
	{
		errors ??= new List<Error>();

		if (errors.Count is 0)
		{
			// maybe log this
			return Problem();
		}

		//this should return a ProblemDetails with the right title from status code
		var problem = Problem(errors.FirstOrDefault());

		if (problem is not ObjectResult objectResult)
		{
			// maybe log this
			return Problem();
		}

		if (objectResult.Value is not ProblemDetails firstProblemDetails)
		{
			// maybe log this
			return Problem();
		}

		foreach (var error in errors)
		{
			firstProblemDetails.Extensions.Add(error.Code, error.Description);
		}

		return objectResult;
	}

	private IActionResult Problem(Error error)
	{
		var statusCode = error.Type switch
		{
			ErrorType.Conflict => StatusCodes.Status409Conflict,
			ErrorType.Validation => StatusCodes.Status400BadRequest,
			ErrorType.NotFound => StatusCodes.Status404NotFound,
			_ => StatusCodes.Status500InternalServerError,
		};

		return Problem(statusCode: statusCode, title: error.Description);
	}
}
