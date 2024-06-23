namespace TimeSheetApp.Api.Concerns.Typicode;

public class TypicodeService : ITypicodeService
{
	private readonly IHttpClientFactory _httpClientFactory;
	private readonly HttpClient _httpClient;

	public TypicodeService(IHttpClientFactory httpClientFactory)
	{
		_httpClientFactory = httpClientFactory;
		_httpClient = _httpClientFactory.CreateClient("TypicodeApi");
	}

	public async Task<IEnumerable<Responses.User>> GetAllUsersAsync()
	{
		var response = await _httpClient.GetAsync("/users");
		var responseBody = await response.Content.ReadFromJsonAsync<IEnumerable<Responses.User>>();
		return responseBody ?? new List<Responses.User>();
	}
}
