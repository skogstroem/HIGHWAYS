using SPACESHIP.Interfaces;

namespace SPACESHIP.Behaviors;

public class DamageBehavior : IBehavior
{
    public void OnCollision(IPlayer player)
    {
        player.LoseHeart();
    }
}

