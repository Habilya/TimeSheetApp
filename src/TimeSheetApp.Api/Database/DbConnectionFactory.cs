using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using TimeSheetApp.Api.Options;

namespace TimeSheetApp.Api.Database;

public class DbConnectionFactory : IDbConnectionFactory
{
	private readonly DbConnectionOptions _connectionOptions;
	public DbConnectionFactory(IOptions<DbConnectionOptions> connectionOptions)
	{
		_connectionOptions = connectionOptions.Value;
	}

	public async Task<IDbConnection> CreateConnectionAsync()
	{
		var connection = new SqlConnection(_connectionOptions.ConnectionString);
		await connection.OpenAsync();
		return connection;
	}
}
