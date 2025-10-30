using HIGHWAYS.Interfaces;

namespace HIGHWAYS.Behaviors;

public class DamageBehavior : IBehavior
{
    private readonly IRenderable _effectRenderer;

    public DamageBehavior(IRenderable effectRenderer)
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

