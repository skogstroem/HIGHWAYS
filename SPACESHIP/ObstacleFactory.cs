using HIGHWAYS.Behaviors;
using HIGHWAYS.GameObjects;
using HIGHWAYS.Interfaces;
using HIGHWAYS.Rendering;

namespace HIGHWAYS.Factories;
public class ObstacleFactory : IGameObjectFactory
{
    private readonly Random _random = new();

    public GameObject CreateGameObject(int x, int y)
    {
        return CreateDebris(x, y);
    }

    private Obstacle CreateDebris(int x, int y)
    {
        var renderer = new AsciiRenderer('▓', ConsoleColor.Gray);
        var behavior = new DamageBehavior();
        return new Obstacle(x, y, renderer, behavior);
    }
}

