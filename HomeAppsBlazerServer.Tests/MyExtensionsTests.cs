using HomeAppsBlazerServer.Components.Extensions;

namespace HomeAppsBlazerServer.Tests;

public class MyExtensionsTests
{
    [Theory]
    [InlineData("hello world", "Hello World",false)]
    [InlineData("CHEESE CLOTH", "Cheese Cloth", false)]
    [InlineData("cottage cheese", "Cottage Cheese", false)]
    [InlineData("good & gather", "Good & Gather", false)]
    [InlineData("GOOD & GATHER", "GOOD & GATHER", true)]
    public void ToTileCase_ConvertsTitleCase(string input, string expected,bool skipalluppercare)
    {
        var result = input.ToTileCase(skipalluppercare);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToTileCase_SingleWord_Capitalizes()
    {
        var result = "milk".ToTileCase();
        Assert.Equal("Milk", result);
    }
}
