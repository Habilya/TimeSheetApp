using TimeSheetApp.Api.Concerns.IndividualMessages;
using TimeSheetApp.Api.Concerns.Users;
using TimeSheetApp.Api.Contracts.Response;

namespace TimeSheetApp.Api.Mapping;

public static class DomainToApiContractMapper
{
	public static UserResponse ToUserResponse(this User user)
	{
		return new UserResponse
		{
			Id = user.Id,
			UserName = user.UserName,
			FirstName = user.FirstName,
			LastName = user.LastName,
			Email = user.Email,
			FullName = user.FullName,
			DateOfBirth = user.DateOfBirth,
			DateCreated = user.DateCreated
		};
	}

	public static GetAllUsersResponse ToUsersResponse(this IEnumerable<User> users)
	{
		return new GetAllUsersResponse
		{
			Users = users.Select(x => x.ToUserResponse())
		};
	}

	public static GetMultipleIndividualMessageResponse ToMultipleIndividualMessageResponse(this IEnumerable<IndividualMessage> individualMessages)
	{
		return new GetMultipleIndividualMessageResponse
		{
			IndividualMessages = individualMessages.Select(x => new IndividualMessageResponse
			{
				Id = x.Id,
				Body = x.Body,
				Subject = x.Subject,
				Version = x.Version,
				SendDate = x.SendDate
			})
		};
	}
}
