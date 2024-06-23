using Microsoft.AspNetCore.Mvc;
using TimeSheetApp.Api.Contracts.Requests;
using TimeSheetApp.Api.Mapping;

namespace TimeSheetApp.Api.Concerns.IndividualMessages;

[Route("individualmessages")]
[ApiController]
public class IndividualMessageController : ControllerBase
{
	private readonly IIndividualMessageService _individualMessageService;

	public IndividualMessageController(IIndividualMessageService individualMessageService)
	{
		_individualMessageService = individualMessageService;
	}


	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		var individualMessages = await _individualMessageService.GetAllAsync();
		var response = individualMessages.ToMultipleIndividualMessageResponse();
		return Ok(response);
	}

	[HttpPost("search")]
	public async Task<IActionResult> Search(IndividualMessageSearchRequest request)
	{
		var individualMessages = await _individualMessageService.SearchAsync(request.SearchString, request.FromDate, request.ToDate);
		if (!individualMessages.Any())
		{
			return NotFound();
		}

		var response = individualMessages.ToMultipleIndividualMessageResponse();
		return Ok(response);
	}
}
