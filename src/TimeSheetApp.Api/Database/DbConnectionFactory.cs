using System.Data;
using System.Data.SqlClient;

namespace TimeSheetApp.Api.Database;

public class DbConnectionFactory : IDbConnectionFactory
{
	private readonly string _connectionString;
	public DbConnectionFactory(string connectionString)
	{
		_connectionString = connectionString;
	}

	public async Task<IDbConnection> CreateConnectionAsync()
	{
		var connection = new SqlConnection(_connectionString);
		await connection.OpenAsync();
		return connection;
	}
}
