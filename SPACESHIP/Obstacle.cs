using HIGHWAYS.Interfaces;

namespace HIGHWAYS.GameObjects;
public class Obstacle : GameObject
{
    public ObstacleType Type { get; }

    public Obstacle(int x, int y, IRender renderer, IBehavior behavior, ObstacleType type)
        : base(x, y, renderer, behavior)
    {
        Type = type;
    }
}

public enum ObstacleType
{
    Debris,
    Bomb
}

