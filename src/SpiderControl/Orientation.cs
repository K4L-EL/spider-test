namespace SpiderControl;

public enum Orientation
{
    Up = 0,
    Right = 1,
    Down = 2,
    Left = 3
}

public static class OrientationExtensions
{
    public static Orientation TurnLeft(this Orientation o) =>
        (Orientation)(((int)o + 3) % 4);

    public static Orientation TurnRight(this Orientation o) =>
        (Orientation)(((int)o + 1) % 4);
}
