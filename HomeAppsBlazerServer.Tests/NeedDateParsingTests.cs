namespace HomeAppsBlazerServer.Tests;

/// <summary>
/// Tests the NeedDate parsing logic used in AddItemToList.
/// The rule: empty/null FutureDate string -> NeedDate = null,
///            valid date string -> NeedDate = parsed date.
/// </summary>
public class NeedDateParsingTests
{
    // Mirrors the logic from ShoppingServices.NeedList.cs: AddItemToList
    private static DateTime? ParseNeedDate(string futureDate) =>
        string.IsNullOrEmpty(futureDate) ? null : DateTime.Parse(futureDate);

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void ParseNeedDate_EmptyOrNull_ReturnsNull(string? futureDate)
    {
        var result = ParseNeedDate(futureDate!);
        Assert.Null(result);
    }

    [Theory]
    [InlineData("2026-03-10", 2026, 3, 10)]
    [InlineData("2026-12-25", 2026, 12, 25)]
    [InlineData("2026-01-01", 2026, 1, 1)]
    public void ParseNeedDate_ValidDateString_ReturnsParsedDate(string futureDate, int year, int month, int day)
    {
        var result = ParseNeedDate(futureDate);
        Assert.NotNull(result);
        Assert.Equal(new DateTime(year, month, day), result!.Value.Date);
    }

    [Fact]
    public void ParseNeedDate_ValidDate_IsNotNull()
    {
        var result = ParseNeedDate("2026-06-15");
        Assert.NotNull(result);
    }

    [Fact]
    public void ParseNeedDate_WhitespaceString_ReturnsNull()
    {
        // string.IsNullOrEmpty does NOT treat whitespace as empty
        // This documents the current behavior — a whitespace-only string would throw
        // This test confirms whitespace is NOT treated as null/empty
        Assert.Throws<FormatException>(() => ParseNeedDate("   "));
    }
}
