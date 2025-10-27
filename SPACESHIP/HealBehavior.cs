using HIGHWAYS.Interfaces;

namespace HIGHWAYS.Behaviors;

public class HealBehavior : IBehavior
{
    public void OnCollision(IPlayer player)
    {
        player.GainHeart();
    }
}

