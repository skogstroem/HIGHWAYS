using SPACESHIP.GameObjects;
using SPACESHIP.Interfaces;

namespace SPACESHIP.Players;

public class AIPlayer : IPlayer
{
    public string Name { get; }
    public int CurrentLane { get; private set; }
    public int Hearts { get; private set; }
    public int Score { get; private set; }
    public bool IsAlive => Hearts > 0;

    private readonly IMovementStrategy _movementStrategy;
    private readonly int _maxLanes;
    private int _updateCounter = 0;
    private const int MaxHearts = 10;

    public AIPlayer(string name, int startLane, int maxLanes, IMovementStrategy movementStrategy)
    {
        Name = name;
        CurrentLane = startLane;
        _maxLanes = maxLanes;
        _movementStrategy = movementStrategy;
        Hearts = 10;
        Score = 0;
    }

    public void MoveToLane(int lane)
    {
        if (lane >= 0 && lane < _maxLanes)
        {
            CurrentLane = lane;
        }
    }
    public void LoseHeart()
    {
        Hearts = Math.Max(0, Hearts - 1);
    }
    public void GainHeart()
    {
        Hearts = Math.Min(MaxHearts, Hearts + 1);
    }
    public void IncreaseScore(int points)
    {
        Score += points;
    }
    public void Update()
    {
        _updateCounter++;
    }
    public void Update(IEnumerable<Lane> lanes)
    {
        _updateCounter++;

        if (_updateCounter % 5 == 0)
        {
            int nextLane = _movementStrategy.DecideNextLane(CurrentLane, lanes, _maxLanes);
            MoveToLane(nextLane);
        }
    }
    public void Render(int yPosition)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.SetCursorPosition(GetXPosition(), yPosition);
        Console.Write("▲");
        Console.ResetColor();
    }
    private int GetXPosition()
    {
        return 35 + (CurrentLane * 4);
    }
}

