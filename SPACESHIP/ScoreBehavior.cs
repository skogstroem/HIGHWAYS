using SPACESHIP.Interfaces;

namespace SPACESHIP.Behaviors;

public class ScoreBehavior : IBehavior
{
    public void OnCollision(IPlayer player)
    {
        player.IncreaseScore(player.Score);
    }
}

