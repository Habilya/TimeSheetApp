using FluentAssertions;
using NSubstitute;
using System.Globalization;
using System.Reflection;
using TimeSheetApp.Api.Concerns.IndividualMessages;
using TimeSheetApp.Api.Database;

namespace TimeSheetApp.Api.Tests.Unit;

public class IndividualMessageRepositoryTests
{
	private readonly IndividualMessageRepository _sut;
	private readonly IDbConnectionFactory _dbConnectionFactory = Substitute.For<IDbConnectionFactory>();

	public IndividualMessageRepositoryTests()
	{
		_sut = new IndividualMessageRepository(_dbConnectionFactory);
	}

	[Theory]
	[InlineData(1, "aaaa", "2001-01-01", "2002-02-02", "SELECT * FROM IndividualMessage WHERE (Subject LIKE @searchStringParam OR Body LIKE @searchStringParam) AND SendDate >= @fromDateParam AND SendDate <= @toDateParam\n")]
	[InlineData(2, "bbb' --", "2001-01-01", "", "SELECT * FROM IndividualMessage WHERE (Subject LIKE @searchStringParam OR Body LIKE @searchStringParam) AND SendDate >= @fromDateParam\n")]
	[InlineData(3, "ccc", "", "", "SELECT * FROM IndividualMessage WHERE (Subject LIKE @searchStringParam OR Body LIKE @searchStringParam)\n")]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1026:Theory methods should use all of their parameters", Justification = "UnitTests with testId")]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "UnitTests with testId")]
	public void BuildDynamicIndividualSearchQuery_ShouldBuildQuery_WhenParamsValid(int id, string searchString, string fromDate, string toDate, string expected)
	{
		// Arrange
		DateTime? DateFromString(string date)
		{
			if (string.IsNullOrWhiteSpace(date))
			{
				return null;
			}

			return DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
		}

		// Private Method info obtained using REFLEXION
		MethodInfo privateMethodBuildDynamicIndividualSearchQuery = typeof(IndividualMessageRepository)
			.GetMethod("BuildDynamicIndividualSearchQuery", BindingFlags.NonPublic | BindingFlags.Instance)!;

		var fromDateParam = DateFromString(fromDate);
		var toDateParam = DateFromString(toDate);

		object[] methodParameters = new object[3] { searchString, fromDateParam!, toDateParam! };

		// Act
		var actual = (Dapper.SqlBuilder.Template)privateMethodBuildDynamicIndividualSearchQuery.Invoke(_sut, methodParameters)!;

		// Assert
		actual.RawSql.Should().Be(expected);
	}
}
