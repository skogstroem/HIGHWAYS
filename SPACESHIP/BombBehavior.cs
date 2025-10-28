using HIGHWAYS.Interfaces;

namespace HIGHWAYS.Behaviors;

public class BombBehavior : IBehavior
{
    public void OnCollision(IPlayer player)
    {
        // tekniskt s�tt skiljer sig detta i beteende och inte i data men det kan ju anses vara r�tt B
        while (player.Hearts > 0)
        {
            player.LoseHeart();
        }
    }
}

