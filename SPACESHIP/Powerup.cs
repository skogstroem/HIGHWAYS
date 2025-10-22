using SPACESHIP.Interfaces;

namespace SPACESHIP.GameObjects;
public class Powerup : GameObject
{
    public PowerupType Type { get; }

    public Powerup(int x, int y, IRenderable renderer, IBehavior behavior, PowerupType type)
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

