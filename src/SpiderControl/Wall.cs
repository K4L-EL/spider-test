namespace SpiderControl;

public readonly record struct Wall(int MaxX, int MaxY)
{
    public bool IsInside(int x, int y) => x >= 0 && y >= 0 && x <= MaxX && y <= MaxY;
}
