using TimeSheetApp.Api.Concerns.Users;
using TimeSheetApp.Api.Contracts.Requests;

namespace TimeSheetApp.Api.Mapping;

public static class ApiContractToDomainMapper
{
	public static User ToUser(this UserCreateRequest request, DateTime createDate)
	{
		return new User
		{
			Id = Guid.NewGuid(),
			UserName = request.UserName,
			FirstName = request.FirstName,
			LastName = request.LastName,
			Email = request.Email,
			DateOfBirth = request.DateOfBirth,
			DateCreated = createDate
		};
	}

	public static User ToUser(this UserUpdateRequest request, Guid id)
	{
		return new User
		{
			Id = id,
			UserName = request.UserName,
			FirstName = request.FirstName,
			LastName = request.LastName,
			Email = request.Email,
			DateOfBirth = request.DateOfBirth,
			DateCreated = request.DateCreated
		};
	}
}
