using HIGHWAYS.Interfaces;

namespace HIGHWAYS.Behaviors;

public class BombBehavior : IBehavior
{
    private readonly IRender _effectRenderer;

    public BombBehavior(IRender effectRenderer)
    {
        _effectRenderer = effectRenderer;
    }

    public void OnCollision(IPlayer player)
    {
        // tekniskt sätt skiljer sig detta i beteende och inte i data men det kan ju anses vara rätt B
        while (player.Hearts > 0)
        {
            player.LoseHeart();
        }
    }

    public void RenderEffect(int x, int y)
    {
        _effectRenderer.Render(x, y);
    }
}

