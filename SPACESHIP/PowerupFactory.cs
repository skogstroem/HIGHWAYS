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
        var effectRenderer = new AsciiRenderer('♥', ConsoleColor.Green);
        var behavior = new HealBehavior(effectRenderer);
        return new Powerup(x, y, renderer, behavior, PowerupType.Health);
    }

    private static Powerup CreateScorePowerup(int x, int y)
    {
        var renderer = new ColoredRenderer('$', ConsoleColor.Cyan, ConsoleColor.Blue);
        var effectRenderer = new AsciiRenderer('★', ConsoleColor.Blue);
        var behavior = new ScoreBehavior(effectRenderer);
        return new Powerup(x, y, renderer, behavior, PowerupType.Score);
    }
}

