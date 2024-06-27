using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using TimeSheetApp.Api.Constants;

namespace TimeSheetApp.Api.Concerns.Base;

[ApiController]
public class ApiController : ControllerBase
{
	protected IActionResult Problem(List<Error> errors)
	{
		HttpContext.Items[HttpContextItemKeys.Errors] = errors;

		var firstError = errors.FirstOrDefault();
		var statusCode = firstError.Type switch
		{
			ErrorType.Conflict => StatusCodes.Status409Conflict,
			ErrorType.Validation => StatusCodes.Status400BadRequest,
			ErrorType.NotFound => StatusCodes.Status404NotFound,
			_ => StatusCodes.Status500InternalServerError,

		};
		return Problem(statusCode: statusCode, title: "One or more application logic errors occurred.");
	}
}
