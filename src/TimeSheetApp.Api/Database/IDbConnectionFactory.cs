using System.Data;

namespace TimeSheetApp.Api.Database;

public interface IDbConnectionFactory
{
	public Task<IDbConnection> CreateConnectionAsync();
}
