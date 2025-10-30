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
        return _random.Next(100) < 5 
            ? CreateBomb(x, y) 
            : CreateDebris(x, y);
    }

    private static Obstacle CreateDebris(int x, int y)
    {
        var renderer = new AsciiRenderer('▓', ConsoleColor.Gray);
        var effectRenderer = new AsciiRenderer('X', ConsoleColor.Red);
        var behavior = new DamageBehavior(effectRenderer);
        return new Obstacle(x, y, renderer, behavior, ObstacleType.Debris);
    }

    private static Obstacle CreateBomb(int x, int y)
    {
        var renderer = new ColoredRenderer('●', ConsoleColor.Red, ConsoleColor.DarkRed);
        var effectRenderer = new AsciiRenderer('*', ConsoleColor.Yellow);
        var behavior = new BombBehavior(effectRenderer);
        return new Obstacle(x, y, renderer, behavior, ObstacleType.Bomb);
    }
}

