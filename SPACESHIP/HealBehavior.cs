using SPACESHIP.Interfaces;

namespace SPACESHIP.Behaviors;

public class HealBehavior : IBehavior
{
    public void OnCollision(IPlayer player)
    {
        player.GainHeart();
    }
}

