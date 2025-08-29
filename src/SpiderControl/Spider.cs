using System;

namespace SpiderControl;

public readonly record struct Position(int X, int Y, Orientation Heading);

public sealed class Spider
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public Orientation Heading { get; private set; }
    private readonly Wall _wall;

    public Spider(int x, int y, Orientation heading, Wall wall)
    {
        _wall = wall;
        if (!_wall.IsInside(x, y))
            throw new ArgumentOutOfRangeException(nameof(x), $"Starting position ({x},{y}) is outside the wall bounds 0,0..{_wall.MaxX},{_wall.MaxY}");
        X = x;
        Y = y;
        Heading = heading;
    }

    public Position Current => new(X, Y, Heading);

    public void TurnLeft() => Heading = Heading.TurnLeft();
    public void TurnRight() => Heading = Heading.TurnRight();

    public void Forward()
    {
        var (nx, ny) = Heading switch
        {
            Orientation.Up => (X, Y + 1),
            Orientation.Right => (X + 1, Y),
            Orientation.Down => (X, Y - 1),
            Orientation.Left => (X - 1, Y),
            _ => (X, Y)
        };
        // Ignore moves that would leave the wall
        if (_wall.IsInside(nx, ny))
        {
            X = nx;
            Y = ny;
        }
    }
}
