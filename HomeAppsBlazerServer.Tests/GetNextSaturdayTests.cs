using HomeAppsBlazerServer.Components.Extensions;

namespace HomeAppsBlazerServer.Tests;

public class GetNextSaturdayTests
{
    [Theory]
    [InlineData("2026-02-02", "2026-02-07")] // Monday -> Saturday
    [InlineData("2026-02-03", "2026-02-07")] // Tuesday -> Saturday
    [InlineData("2026-02-04", "2026-02-07")] // Wednesday -> Saturday
    [InlineData("2026-02-05", "2026-02-07")] // Thursday -> Saturday
    [InlineData("2026-02-06", "2026-02-07")] // Friday -> Saturday
    [InlineData("2026-02-07", "2026-02-14")] // Saturday -> NEXT Saturday (skips today)
    [InlineData("2026-02-08", "2026-02-14")] // Sunday -> next Saturday
    public void GetNextSaturday_ReturnsCorrectDate(string inputDate, string expectedDate)
    {
        var from = DateTime.Parse(inputDate);
        var expected = DateTime.Parse(expectedDate);

        var result = MyExtensions.GetNextSaturday(from);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetNextSaturday_WhenSaturday_ReturnsNextWeek()
    {
        // If today is Saturday, it should return NEXT Saturday, not today
        var saturday = new DateTime(2026, 2, 7); // a Saturday

        var result = MyExtensions.GetNextSaturday(saturday);

        Assert.Equal(DayOfWeek.Saturday, result.DayOfWeek);
        Assert.Equal(saturday.AddDays(7), result);
    }

    [Fact]
    public void GetNextSaturday_AlwaysReturnsSaturday()
    {
        // Test every day of a week to make sure result is always a Saturday
        for (int i = 0; i < 7; i++)
        {
            var date = new DateTime(2026, 2, 2).AddDays(i); // Mon Feb 2 through Sun Feb 8
            var result = MyExtensions.GetNextSaturday(date);
            Assert.Equal(DayOfWeek.Saturday, result.DayOfWeek);
        }
    }
}
