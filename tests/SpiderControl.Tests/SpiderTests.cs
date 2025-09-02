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

    [Fact]
    public void Requirements_Example_Test_Input_Produces_Expected_Output()
    {
        var wall = Parser.ParseWall("7 15");
        var startPosition = Parser.ParsePosition("4 10 Left", wall);
        var commands = Parser.ParseCommands("FLFLFRFFLF");

        var finalPosition = CommandProcessor.Execute(wall, startPosition, commands);

        Assert.Equal(5, finalPosition.X);
        Assert.Equal(7, finalPosition.Y);
        Assert.Equal(Orientation.Right, finalPosition.Heading);
    }

    [Fact]
    public void Complete_Input_Output_Flow_Test()
    {
        var wallInput = "7 15";
        var positionInput = "4 10 Left";
        var commandsInput = "FLFLFRFFLF";

        var wall = Parser.ParseWall(wallInput);
        var start = Parser.ParsePosition(positionInput, wall);
        var commands = Parser.ParseCommands(commandsInput);

        var final = CommandProcessor.Execute(wall, start, commands);

        var expectedOutput = "5 7 Right";
        var actualOutput = $"{final.X} {final.Y} {final.Heading}";
        
        Assert.Equal(expectedOutput, actualOutput);
    }

    [Theory]
    [InlineData("5 5", "2 2 Up", "FFFRFFFRFFF", "5 2 Down")]
    [InlineData("3 3", "0 0 Right", "FFRFFRFFRFF", "0 2 Up")]
    [InlineData("1 1", "1 1 Down", "LLFF", "1 1 Up")]
    public void Various_Input_Output_Scenarios(string wallInput, string posInput, string cmdInput, string expectedOutput)
    {
        var wall = Parser.ParseWall(wallInput);
        var start = Parser.ParsePosition(posInput, wall);
        var commands = Parser.ParseCommands(cmdInput);

        var final = CommandProcessor.Execute(wall, start, commands);
        var actualOutput = $"{final.X} {final.Y} {final.Heading}";

        Assert.Equal(expectedOutput, actualOutput);
    }

    [Fact]
    public void Spider_Cannot_Move_Outside_Wall_Boundaries()
    {
        var wall = Parser.ParseWall("2 2");
        var start = Parser.ParsePosition("0 0 Left", wall);
        var commands = Parser.ParseCommands("FFFF");

        var final = CommandProcessor.Execute(wall, start, commands);

        Assert.Equal(0, final.X);
        Assert.Equal(0, final.Y);
        Assert.Equal(Orientation.Left, final.Heading);
    }

    [Fact]
    public void Spider_Turns_But_Stays_In_Position()
    {
        var wall = Parser.ParseWall("5 5");
        var start = Parser.ParsePosition("2 2 Up", wall);
        var commands = Parser.ParseCommands("LLLL");

        var final = CommandProcessor.Execute(wall, start, commands);

        Assert.Equal(2, final.X);
        Assert.Equal(2, final.Y);
        Assert.Equal(Orientation.Up, final.Heading);
    }}
