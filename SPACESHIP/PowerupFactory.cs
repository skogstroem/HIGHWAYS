using HIGHWAYS.Behaviors;
using HIGHWAYS.GameObjects;
using HIGHWAYS.Interfaces;
using HIGHWAYS.Rendering;

namespace HIGHWAYS.Factories;
public class PowerupFactory : IGameObjectFactory
{
    private readonly Random _random = new();

    public GameObject CreateGameObject(int x, int y)
    {
        return _random.Next(100) < 20 
            ? CreateScorePowerup(x, y) 
            : CreateHealthPowerup(x, y);
    }

    private static Powerup CreateHealthPowerup(int x, int y)
    {
        var renderer = new ColoredRenderer('+', ConsoleColor.Green, ConsoleColor.DarkGreen);
        var behavior = new HealBehavior();
        return new Powerup(x, y, renderer, behavior, PowerupType.Health);
    }

    private static Powerup CreateScorePowerup(int x, int y)
    {
        var renderer = new ColoredRenderer('$', ConsoleColor.Cyan, ConsoleColor.Blue);
        var behavior = new ScoreBehavior();
        return new Powerup(x, y, renderer, behavior, PowerupType.Score);
    }
}

