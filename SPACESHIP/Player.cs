using HIGHWAYS.Interfaces;

namespace HIGHWAYS.Players;

public class Player : IPlayer
{
    public string Name { get; }
    public int CurrentLane { get; private set; }
    public int Hearts { get; private set; }
    public int Score { get; private set; }
    public bool IsAlive => Hearts > 0;

    private readonly int _maxLanes;
    private const int MaxHearts = 10;

    public Player(string name, int startLane, int maxLanes)
    {
        Name = name;
        CurrentLane = startLane;
        _maxLanes = maxLanes;
        Hearts = 3; 
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
        while (Console.KeyAvailable)
        {
            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                case ConsoleKey.A:
                    MoveToLane(CurrentLane - 1);
                    break;
                case ConsoleKey.RightArrow:
                case ConsoleKey.D:
                    MoveToLane(CurrentLane + 1);
                    break;
            }
        }
    }

    public void Render(int yPosition)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.SetCursorPosition(GetXPosition(), yPosition);
        Console.Write("▲");
        Console.ResetColor();
    }

    private int GetXPosition()
    {
        return 5 + (CurrentLane * 4);
    }
}

