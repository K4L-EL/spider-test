using SpiderControl;
using Xunit;

namespace SpiderControl.Tests;

public class SpiderTests
{
    [Fact]
    public void Example_From_Spec_Works()
    {
        // Wall: 0,0 .. 7,15
        var wall = new Wall(7, 15);
        var start = new Position(4, 10, Orientation.Left);
        var commands = "FLFLFRFFLF";

        var final = CommandProcessor.Execute(wall, start, commands);
        Assert.Equal(5, final.X);
        Assert.Equal(7, final.Y);
        Assert.Equal(Orientation.Right, final.Heading);
    }

    [Fact]
    public void Ignores_Moves_That_Would_Leave_Wall()
    {
        var wall = new Wall(1, 1);
        var start = new Position(0, 0, Orientation.Left); // facing outwards (negative X)
        var final = CommandProcessor.Execute(wall, start, "F"); // would go to (-1,0) -> ignored
        Assert.Equal(0, final.X);
        Assert.Equal(0, final.Y);
        Assert.Equal(Orientation.Left, final.Heading);
    }

    [Theory]
    [InlineData("0 0", 0, 0)]
    [InlineData("7 15", 7, 15)]
    public void ParseWall_Works(string line, int ex, int ey)
    {
        var wall = Parser.ParseWall(line);
        Assert.Equal(ex, wall.MaxX);
        Assert.Equal(ey, wall.MaxY);
    }

    [Theory]
    [InlineData("4 10 Left", 4, 10, Orientation.Left)]
    [InlineData("0 0 Up", 0, 0, Orientation.Up)]
    [InlineData("7 15 Right", 7, 15, Orientation.Right)]
    [InlineData("3 2 down", 3, 2, Orientation.Down)]
    public void ParsePosition_Works(string line, int x, int y, Orientation o)
    {
        var wall = new Wall(7, 15);
        var pos = Parser.ParsePosition(line, wall);
        Assert.Equal(x, pos.X);
        Assert.Equal(y, pos.Y);
        Assert.Equal(o, pos.Heading);
    }
}
