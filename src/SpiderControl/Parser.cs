using System;
using System.Globalization;

namespace SpiderControl;

public static class Parser
{
    public static Wall ParseWall(string line)
    {
        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (parts.Length != 2 || !int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out var maxX) ||
            !int.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out var maxY) ||
            maxX < 0 || maxY < 0)
        {
            throw new FormatException("First line must be two non-negative integers: \"<maxX> <maxY>\".");
        }
        return new Wall(maxX, maxY);
    }

    public static Position ParsePosition(string line, Wall wall)
    {
        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (parts.Length != 3)
            throw new FormatException("Second line must be: \"<x> <y> <orientation>\" (e.g., 4 10 Left).");

        if (!int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out var x) ||
            !int.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out var y))
            throw new FormatException("x and y must be integers.");

        var orientationStr = parts[2].ToLowerInvariant();
        Orientation o = orientationStr switch
        {
            "up" => Orientation.Up,
            "right" => Orientation.Right,
            "down" => Orientation.Down,
            "left" => Orientation.Left,
            _ => throw new FormatException("Orientation must be one of: Up, Right, Down, Left.")
        };

        if (!wall.IsInside(x, y))
            throw new ArgumentOutOfRangeException(nameof(line), $"Start position ({x},{y}) is outside wall 0,0..{wall.MaxX},{wall.MaxY}.");

        return new Position(x, y, o);
    }

    public static string ParseCommands(string line)
    {
        // Validate only L, R, F plus whitespace
        foreach (var c in line)
        {
            if (char.IsWhiteSpace(c)) continue;
            if (c is not ('L' or 'R' or 'F'))
                throw new FormatException("Commands must contain only 'L', 'R', 'F' characters.");
        }
        return line.Trim();
    }
}
