using TimeSheetApp.Api.Contracts.Data;
using TimeSheetApp.Api.Domain;

namespace TimeSheetApp.Api.Mapping;

public static class DomainToDtoMapper
{
	public static UserDto ToUserDto(this User user)
	{
		return new UserDto
		{
			id = user.Id,
			username = user.UserName,
			first_name = user.FirstName,
			last_name = user.LastName,
			email = user.Email,
			date_of_birth = user.DateOfBirth,
			date_created = user.DateCreated
		};
	}
}
