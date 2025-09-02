using SpiderControl;
using Xunit;

namespace SpiderControl.Tests;

public class InputValidationTests
{
    [Theory]
    [InlineData("")]
    [InlineData("7")]
    [InlineData("7 15 extra")]
    [InlineData("abc def")]
    [InlineData("-1 5")]
    [InlineData("5 -1")]
    public void ParseWall_ThrowsFormatException_ForInvalidInput(string invalidInput)
    {
        Assert.Throws<FormatException>(() => Parser.ParseWall(invalidInput));
    }

    [Theory]
    [InlineData("")]
    [InlineData("4 10")]
    [InlineData("4 10 Left extra")]
    [InlineData("abc 10 Left")]
    [InlineData("4 abc Left")]
    [InlineData("4 10 InvalidOrientation")]
    public void ParsePosition_ThrowsFormatException_ForInvalidInput(string invalidInput)
    {
        var wall = new Wall(10, 10);
        Assert.Throws<FormatException>(() => Parser.ParsePosition(invalidInput, wall));
    }

    [Fact]
    public void ParsePosition_ThrowsArgumentOutOfRangeException_WhenPositionOutsideWall()
    {
        var wall = new Wall(5, 5);
        Assert.Throws<ArgumentOutOfRangeException>(() => Parser.ParsePosition("10 10 Up", wall));
    }

    [Theory]
    [InlineData("FLXFLF")]
    [InlineData("123")]
    [InlineData("flf")]
    public void ParseCommands_ThrowsFormatException_ForInvalidCommands(string invalidCommands)
    {
        Assert.Throws<FormatException>(() => Parser.ParseCommands(invalidCommands));
    }

    [Theory]
    [InlineData("FLF")]
    [InlineData("LLLRRR")]
    [InlineData("F F F")]
    [InlineData(" FLF ")]
    [InlineData("")]
    public void ParseCommands_AcceptsValidCommands(string validCommands)
    {
        var result = Parser.ParseCommands(validCommands);
        Assert.NotNull(result);
    }

    [Fact]
    public void Spider_ThrowsArgumentOutOfRangeException_WhenCreatedOutsideWall()
    {
        var wall = new Wall(5, 5);
        Assert.Throws<ArgumentOutOfRangeException>(() => new Spider(10, 10, Orientation.Up, wall));
    }

    [Theory]
    [InlineData("up")]
    [InlineData("UP")]
    [InlineData("Up")]
    [InlineData("right")]
    [InlineData("RIGHT")]
    [InlineData("Right")]
    [InlineData("down")]
    [InlineData("DOWN")]
    [InlineData("Down")]
    [InlineData("left")]
    [InlineData("LEFT")]
    [InlineData("Left")]
    public void ParsePosition_AcceptsCaseInsensitiveOrientations(string orientation)
    {
        var wall = new Wall(10, 10);
        var input = $"5 5 {orientation}";
        
        // Should not throw
        var result = Parser.ParsePosition(input, wall);
        Assert.Equal(5, result.X);
        Assert.Equal(5, result.Y);
    }
}
