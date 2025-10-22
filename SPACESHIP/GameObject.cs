using SPACESHIP.Interfaces;

namespace SPACESHIP.GameObjects;
public abstract class GameObject
{
    public int X { get; set; }
    public int Y { get; set; }
    public bool IsActive { get; set; } = true;

    protected IRenderable Renderer { get; set; }

    protected IBehavior Behavior { get; set; }

    protected GameObject(int x, int y, IRenderable renderer, IBehavior behavior)
    {
        X = x;
        Y = y;
        Renderer = renderer;
        Behavior = behavior;
    }

    public virtual void Update()
    {
        Y++;
    }

    public void Render()
    {
        if (IsActive && Y >= 0 && Y < 30)
        {
            Renderer.Render(X, Y);
        }
    }

    public void HandleCollision(IPlayer player)
    {
        Behavior.OnCollision(player);
        IsActive = false;
    }

    public bool CollidesWith(int playerX, int playerY)
    {
        return IsActive && X == playerX && Y == playerY;
    }
}

