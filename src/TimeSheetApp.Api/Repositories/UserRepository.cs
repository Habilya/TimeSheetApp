using Dapper;
using TimeSheetApp.Api.Contracts.Data;
using TimeSheetApp.Api.Database;

namespace TimeSheetApp.Api.Repositories;

public class UserRepository : IUserRepository
{
	private readonly IDbConnectionFactory _connectionFactory;

	public UserRepository(IDbConnectionFactory connectionFactory)
	{
		_connectionFactory = connectionFactory;
	}

	public async Task<IEnumerable<UserDto>> GetAllAsync()
	{
		using var connection = await _connectionFactory.CreateConnectionAsync();
		return await connection.QueryAsync<UserDto>("SELECT * FROM Users");
	}

	public async Task<UserDto?> GetAsync(Guid id)
	{
		using var connection = await _connectionFactory.CreateConnectionAsync();
		return await connection.QuerySingleOrDefaultAsync<UserDto>(
			"SELECT * FROM Users WHERE id = @Id",
			new { Id = id }
		);
	}

	public async Task<bool> CreateAsync(UserDto user)
	{
		using var connection = await _connectionFactory.CreateConnectionAsync();
		var result = await connection.ExecuteAsync(
			@"INSERT INTO Users (id, username, first_name, last_name, email, date_of_birth, date_created) 
			VALUES (@id, @username, @first_name, @last_name, @email, @date_of_birth, @date_created)",
			user);
		return result > 0;
	}

	public async Task<bool> UpdateAsync(UserDto user)
	{
		using var connection = await _connectionFactory.CreateConnectionAsync();
		var result = await connection.ExecuteAsync(
			@"UPDATE Users SET 
					username = @username,
					first_name = @first_name,
					last_name = @last_name, 
					email = @email,
					date_of_birth = @date_of_birth,
					date_created = @date_created 
				WHERE id = @id",
			user
		);
		return result > 0;
	}

	public async Task<bool> DeleteAsync(Guid id)
	{
		using var connection = await _connectionFactory.CreateConnectionAsync();
		var result = await connection.ExecuteAsync(
			@"DELETE FROM Users WHERE id = @Id",
			new { Id = id }
		);
		return result > 0;
	}
}
