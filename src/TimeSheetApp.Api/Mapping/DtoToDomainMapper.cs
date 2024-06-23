using TimeSheetApp.Api.Concerns.IndividualMessages;
using TimeSheetApp.Api.Concerns.Users;
using TimeSheetApp.Api.Contracts.Data;

namespace TimeSheetApp.Api.Mapping;

public static class DtoToDomainMapper
{
	public static User ToUser(this UserDto userDto)
	{
		return new User
		{
			Id = userDto.id,
			UserName = userDto.username,
			FirstName = userDto.first_name,
			LastName = userDto.last_name,
			Email = userDto.email,
			DateOfBirth = userDto.date_of_birth,
			DateCreated = userDto.date_created
		};
	}

	public static IndividualMessage ToIndividualMessage(this IndividualMessageDto individualMessageDto)
	{
		return new IndividualMessage
		{
			Id = individualMessageDto.Id,
			Version = individualMessageDto.Version,
			Subject = individualMessageDto.Subject,
			Body = individualMessageDto.Body,
			SendDate = individualMessageDto.SendDate
		};
	}
}
