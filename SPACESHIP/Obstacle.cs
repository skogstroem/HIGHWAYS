using HIGHWAYS.Interfaces;

namespace HIGHWAYS.GameObjects;

public class Obstacle : GameObject
{
    public Obstacle(int x, int y, IRenderable renderer, IBehavior behavior)
        : base(x, y, renderer, behavior)
    {
    }
}

