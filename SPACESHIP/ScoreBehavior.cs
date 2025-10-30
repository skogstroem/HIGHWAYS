using HIGHWAYS.Interfaces;

namespace HIGHWAYS.Behaviors;

public class ScoreBehavior : IBehavior
{
    private readonly IRenderable _effectRenderer;

    public ScoreBehavior(IRenderable effectRenderer)
    {
        _effectRenderer = effectRenderer;
    }

    public void OnCollision(IPlayer player)
    {
        player.IncreaseScore(player.Score);
    }

    public void RenderEffect(int x, int y)
    {
        _effectRenderer.Render(x, y);
    }
}

