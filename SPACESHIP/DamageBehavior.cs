using HIGHWAYS.Interfaces;

namespace HIGHWAYS.Behaviors;

public class DamageBehavior : IBehavior
{
    private readonly IRender _effectRenderer;

    public DamageBehavior(IRender effectRenderer)
    {
        _effectRenderer = effectRenderer;
    }

    public void OnCollision(IPlayer player)
    {
        player.LoseHeart();
    }

    public void RenderEffect(int x, int y)
    {
        _effectRenderer.Render(x, y);
    }
}

