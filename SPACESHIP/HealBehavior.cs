using HIGHWAYS.Interfaces;

namespace HIGHWAYS.Behaviors;

public class HealBehavior : IBehavior
{
    private readonly IRenderable _effectRenderer;

    public HealBehavior(IRenderable effectRenderer)
    {
        _effectRenderer = effectRenderer;
    }

    public void OnCollision(IPlayer player)
    {
        player.GainHeart();
    }

    public void RenderEffect(int x, int y)
    {
        _effectRenderer.Render(x, y);
    }
}

