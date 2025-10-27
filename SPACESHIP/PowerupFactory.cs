using HIGHWAYS.Behaviors;
using HIGHWAYS.GameObjects;
using HIGHWAYS.Interfaces;
using HIGHWAYS.Rendering;

namespace HIGHWAYS.Factories;

public class PowerupFactory : IGameObjectFactory
{
    private readonly Random _random = new();
    private readonly PowerupType _type;

    public PowerupFactory(PowerupType type)
    {
        _type = type;
    }

    public GameObject CreateGameObject(int x, int y)
    {
        return _type switch
        {
            PowerupType.Health => CreateHealthPowerup(x, y),
            PowerupType.Score => CreateScorePowerup(x, y),
            _ => CreateHealthPowerup(x, y)
        };
    }

    private Powerup CreateHealthPowerup(int x, int y)
    {
        var renderer = new ColoredRenderer('+', ConsoleColor.Green, ConsoleColor.DarkGreen);
        var behavior = new HealBehavior();
        return new Powerup(x, y, renderer, behavior, PowerupType.Health);
    }

    private Powerup CreateScorePowerup(int x, int y)
    {
        var renderer = new ColoredRenderer('$', ConsoleColor.Cyan, ConsoleColor.Blue);
        var behavior = new ScoreBehavior();
        return new Powerup(x, y, renderer, behavior, PowerupType.Score);
    }
}

