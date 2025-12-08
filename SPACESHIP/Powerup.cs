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
    
    public override void HandleStreak(IPlayer player)
    {
        if (player.Streak >= 3)
        {
            player.Score =+ 10000;
            player.Streak++;
        }
        else
            player.Streak++;
    }
}

public enum PowerupType
{
    Health,
    Score
}

