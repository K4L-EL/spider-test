using System;
using System.Text;

namespace SpiderControl;

public static class CommandProcessor
{
    public static Position Execute(Wall wall, Position start, ReadOnlySpan<char> commands)
    {
        var spider = new Spider(start.X, start.Y, start.Heading, wall);
        foreach (var c in commands)
        {
            switch (c)
            {
                case 'L': spider.TurnLeft(); break;
                case 'R': spider.TurnRight(); break;
                case 'F': spider.Forward(); break;
                case ' ':
                case '\t':
                case '\r':
                case '\n':
                    break;
                default:
                    throw new ArgumentException($"Invalid command character: '{c}'. Allowed: L, R, F.");
            }
        }
        return spider.Current;
    }
}
