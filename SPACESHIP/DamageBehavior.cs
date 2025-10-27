using HIGHWAYS.Interfaces;

namespace HIGHWAYS.Behaviors;

public class DamageBehavior : IBehavior
{
    public void OnCollision(IPlayer player)
    {
        player.LoseHeart();
    }
}

