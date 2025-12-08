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

    public override void HandleStreak (IPlayer player)
    {
        if (player.Streak <= 3 && player.Score - 10000 > 0)
        {
             player.Score =- 10000;
             player.Streak = 0;
        }
        else
        {
             player.Streak = 0;
             player.Score = 0;
        }
           
    }
}

public enum ObstacleType
{
    Debris,
    Bomb
}

