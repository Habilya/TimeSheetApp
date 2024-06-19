using TimeSheetApp.Api.Contracts.Requests;
using TimeSheetApp.Api.Domain;

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

	public static User ToUser(this UserUpdateRequest request)
	{
		return new User
		{
			Id = request.Id,
			UserName = request.UserName,
			FirstName = request.FirstName,
			LastName = request.LastName,
			Email = request.Email,
			DateOfBirth = request.DateOfBirth,
			DateCreated = request.DateCreated
		};
	}
}
