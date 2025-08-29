using SpiderControl;

var wallLine = Console.ReadLine() ?? string.Empty;
var posLine = Console.ReadLine() ?? string.Empty;
var cmdLine = Console.ReadLine() ?? string.Empty;

try
{
    var wall = Parser.ParseWall(wallLine);
    var start = Parser.ParsePosition(posLine, wall);
    var cmds = Parser.ParseCommands(cmdLine);

    var final = CommandProcessor.Execute(wall, start, cmds);
    Console.WriteLine($"{final.X} {final.Y} {final.Heading}");
}
catch (Exception ex)
{
    Console.Error.WriteLine(ex.Message);
    Environment.ExitCode = 1;
}
