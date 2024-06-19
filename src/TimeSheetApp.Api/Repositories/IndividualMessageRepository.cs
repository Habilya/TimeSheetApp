using Dapper;
using TimeSheetApp.Api.Contracts.Data;
using TimeSheetApp.Api.Database;

namespace TimeSheetApp.Api.Repositories;

public class IndividualMessageRepository : IIndividualMessageRepository
{
	private readonly IDbConnectionFactory _dbConnectionFactory;

	public IndividualMessageRepository(IDbConnectionFactory dbConnectionFactory)
	{
		_dbConnectionFactory = dbConnectionFactory;
	}

	public async Task<IEnumerable<IndividualMessageDto>> GetAllAsync()
	{
		using var connection = await _dbConnectionFactory.CreateConnectionAsync();
		return await connection.QueryAsync<IndividualMessageDto>("SELECT * FROM IndividualMessage");
	}

	public async Task<IEnumerable<IndividualMessageDto>> SearchAsync(string searchString, DateTime? fromDate, DateTime? toDate)
	{
		using var connection = await _dbConnectionFactory.CreateConnectionAsync();
		var selector = BuildDynamicIndividualSearchQuery(searchString, fromDate, toDate);
		return await connection.QueryAsync<IndividualMessageDto>(selector.RawSql, selector.Parameters);
	}

	private SqlBuilder.Template BuildDynamicIndividualSearchQuery(string searchString, DateTime? fromDate, DateTime? toDate)
	{
		var builder = new SqlBuilder();

		//note the 'where' in-line comment is required, it is a replacement token
		var selector = builder.AddTemplate("SELECT * FROM IndividualMessage /**where**/");

		builder.Where("(Subject LIKE @searchStringParam OR Body LIKE @searchStringParam)", new { searchStringParam = $"%{searchString}%" });

		if (fromDate.HasValue)
		{
			builder.Where("SendDate >= @fromDateParam", new { fromDateParam = fromDate });
		}

		if (toDate.HasValue)
		{
			builder.Where("SendDate <= @toDateParam", new { toDateParam = toDate });
		}

		return selector;
	}
}
