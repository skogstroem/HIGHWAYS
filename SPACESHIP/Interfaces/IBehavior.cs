namespace HIGHWAYS.Interfaces;

public interface IBehavior
{
    void OnCollision(IPlayer player);
    void RenderEffect(int x, int y);
}

