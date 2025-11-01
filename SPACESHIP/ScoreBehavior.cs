using HIGHWAYS.Interfaces;

namespace HIGHWAYS.Behaviors;

public class ScoreBehavior : IBehavior
{
    private readonly IRender _effectRenderer;

    public ScoreBehavior(IRender effectRenderer)
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

