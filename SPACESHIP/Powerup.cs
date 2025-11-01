using HIGHWAYS.Interfaces;

namespace HIGHWAYS.GameObjects;
public class Powerup : GameObject
{
    public PowerupType Type { get; }

    public Powerup(int x, int y, IRender renderer, IBehavior behavior, PowerupType type)
        : base(x, y, renderer, behavior)
    {
        Type = type;
    }
}

public enum PowerupType
{
    Health,
    Score
}

