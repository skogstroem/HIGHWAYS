using HIGHWAYS.Interfaces;

namespace HIGHWAYS.Behaviors;

public class ScoreBehavior : IBehavior
{
    public void OnCollision(IPlayer player)
    {
        player.IncreaseScore(player.Score);
    }
}

