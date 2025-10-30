using HIGHWAYS.GameObjects;
using HIGHWAYS.Players;
using HIGHWAYS.Generics;
using HIGHWAYS.Interfaces;

namespace HIGHWAYS.Core;

public class Game
{
    private const int NumLanes = 5;
    private const int ScreenWidth = 80;
    private const int ScreenHeight = 30;
    private const int PlayerYPosition = ScreenHeight - 3;
    private const int LeftPlayfieldOffset = 5;
    private const int RightPlayfieldOffset = 42;
    private readonly List<Lane> _playerLanes;
    private readonly List<Lane> _aiLanes;
    private readonly HumanPlayer _humanPlayer;
    private readonly AIPlayer? _aiPlayer;
    private readonly IGameObjectFactory _obstacleFactory;
    private readonly IGameObjectFactory _powerupFactory;
    private readonly Random _random;
    private readonly ObjectBuffer<string> _messageEvents;
    private int _rowCounter;
    private const int RowsPerSpawn = 5;
    private GameObject? _lastCollisionObject;

    public Game(HumanPlayer humanPlayer, 
                AIPlayer? aiPlayer,
                IGameObjectFactory obstacleFactory,
                IGameObjectFactory powerupFactory)
    {
        _humanPlayer = humanPlayer ?? throw new ArgumentNullException(nameof(humanPlayer));
        _aiPlayer = aiPlayer;
        _obstacleFactory = obstacleFactory ?? throw new ArgumentNullException(nameof(obstacleFactory));
        _powerupFactory = powerupFactory ?? throw new ArgumentNullException(nameof(powerupFactory));
        _messageEvents = new ObjectBuffer<string>(10);

        _playerLanes = new List<Lane>();
        for (int i = 0; i < NumLanes; i++)
        {
            int xPos = LeftPlayfieldOffset + (i * 4);
            _playerLanes.Add(new Lane(i, xPos));
        }

        
        _aiLanes = new List<Lane>();
        if (_aiPlayer != null)
        {
            for (int i = 0; i < NumLanes; i++)
            {
                int xPos = RightPlayfieldOffset + (i * 4);
                _aiLanes.Add(new Lane(i, xPos));
            }
        }

        _random = new Random();
        _rowCounter = 0;
    }

    public void Update()
    {
        _humanPlayer.Update();
        _aiPlayer?.Update(_aiLanes);

        foreach (var lane in _playerLanes)
        {
            lane.UpdateAll();
        }

        if (_aiPlayer != null)
        {
            foreach (var lane in _aiLanes)
            {
                lane.UpdateAll();
            }
        }

        SpawnObjects();
        CheckCollisions();
        CleanupObjects();
        _humanPlayer.IncreaseScore(1);
        _aiPlayer?.IncreaseScore(1);
    }

    private void SpawnObjects()
    {
        _rowCounter++;

        if (_rowCounter % RowsPerSpawn == 0)
        {
            SpawnRow();
        }
    }

    private void SpawnRow()
    {
        var availableLanes = Enumerable.Range(0, NumLanes).ToList();
        var selectedObstacleLanes = new List<int>();
        int? powerupLane = null;
        
        for (int i = 0; i < 2 && availableLanes.Count > 0; i++)
        {
            int laneIndexPosition = _random.Next(availableLanes.Count);
            int laneIndex = availableLanes[laneIndexPosition];
            availableLanes.RemoveAt(laneIndexPosition);
            selectedObstacleLanes.Add(laneIndex);
        }
        
        if (_random.Next(0, 100) < 25 && availableLanes.Count > 0)
        {
            int laneIndexPosition = _random.Next(availableLanes.Count);
            powerupLane = availableLanes[laneIndexPosition];
        }

        foreach (var laneIndex in selectedObstacleLanes)
        {
            var lane = _playerLanes[laneIndex];
            var obj = _obstacleFactory.CreateGameObject(lane.XPosition, 0);
            lane.AddObject(obj);
        }
        
        if (powerupLane.HasValue)
        {
            var lane = _playerLanes[powerupLane.Value];
            var powerup = _powerupFactory.CreateGameObject(lane.XPosition, 0);
            lane.AddObject(powerup);
        }

        if (_aiPlayer != null)
        {
            foreach (var laneIndex in selectedObstacleLanes)
            {
                var lane = _aiLanes[laneIndex];
                var obj = _obstacleFactory.CreateGameObject(lane.XPosition, 0);
                lane.AddObject(obj);
            }
            
            if (powerupLane.HasValue)
            {
                var lane = _aiLanes[powerupLane.Value];
                var powerup = _powerupFactory.CreateGameObject(lane.XPosition, 0);
                lane.AddObject(powerup);
            }
        }
    }

    private void CheckCollisions()
    {
        var humanLane = _playerLanes[_humanPlayer.CurrentLane];
        var humanCollisions = humanLane
            .Where(obj => obj.IsActive)
            .Where(obj => Math.Abs(obj.Y - PlayerYPosition) <= 1)
            .ToList();

        foreach (var obj in humanCollisions)
        {
            if (obj is Powerup powerup)
            {
                _messageEvents.TryAdd($"       LETS GO!");
            }
            else if (obj is Obstacle obstacle)
            {
                if (obstacle.Type != ObstacleType.Bomb)
                {
                    _messageEvents.TryAdd("         AUCH!");
                }
                else
                {
                }
            }
            
            _lastCollisionObject = obj;
            obj.HandleCollision(_humanPlayer);
        }

        if (_aiPlayer != null)
        {
            var aiLane = _aiLanes[_aiPlayer.CurrentLane];
            var aiCollisions = aiLane
                .Where(obj => obj.IsActive)
                .Where(obj => Math.Abs(obj.Y - PlayerYPosition) <= 1)
                .ToList();

            foreach (var obj in aiCollisions)
            {
                obj.HandleCollision(_aiPlayer);
            }
        }
    }

    private void CleanupObjects()
    {
        foreach (var lane in _playerLanes)
        {
            lane.RemoveInactiveObjects();
            lane.RemoveWhere(obj => obj.Y < -5 || obj.Y > ScreenHeight);
        }

        if (_aiPlayer != null)
        {
            foreach (var lane in _aiLanes)
            {
                lane.RemoveInactiveObjects();
                lane.RemoveWhere(obj => obj.Y < -5 || obj.Y > ScreenHeight);
            }
        }

        var totalActiveObjects = _playerLanes
            .SelectMany(lane => lane)
            .Count(obj => obj.IsActive);

        var obstacleCount = _playerLanes
            .SelectMany(lane => lane)
            .OfType<Obstacle>()
            .Count(obj => obj.IsActive);

        var powerupCount = _playerLanes
            .SelectMany(lane => lane)
            .OfType<Powerup>()
            .Count(obj => obj.IsActive);
    }

    public void DrawInitialFrame()
    {
        DrawGameArea();
        DrawLanes();
    }

    public void Render()
    {
        ClearPlayArea();

        foreach (var lane in _playerLanes)
        {
            lane.RenderAll();
        }

        _humanPlayer.Render(PlayerYPosition);

        if (_aiPlayer != null)
        {
            foreach (var lane in _aiLanes)
            {
                lane.RenderAll();
            }
            
            _aiPlayer.Render(PlayerYPosition);
        }

        DrawUI();
    }

    private void ClearPlayArea()
    {
        for (int laneNum = 0; laneNum < NumLanes; laneNum++)
        {
            int laneX = LeftPlayfieldOffset + (laneNum * 4);
            for (int y = 1; y < ScreenHeight - 1; y++)
            {
                Console.SetCursorPosition(laneX, y);
                Console.Write("  ");
            }
        }

        if (_aiPlayer != null)
        {
            for (int laneNum = 0; laneNum < NumLanes; laneNum++)
            {
                int laneX = RightPlayfieldOffset + (laneNum * 4);
                for (int y = 1; y < ScreenHeight - 1; y++)
                {
                    Console.SetCursorPosition(laneX, y);
                    Console.Write("  ");
                }
            }
        }
    }

    private void DrawGameArea()
    {
        for (int y = 0; y < ScreenHeight; y++)
        {
            Console.SetCursorPosition(0, y);
            Console.Write("║");
        }
    }

    private void DrawLanes()
    {
        for (int i = -1; i < NumLanes; i++)
        {
            int x = LeftPlayfieldOffset + (i * 4) + 2;
            for (int y = 1; y < ScreenHeight - 1; y++)
            {
                Console.SetCursorPosition(x, y);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("|");
                Console.ResetColor();
            }
        }

        if (_aiPlayer != null)
        {
            int separatorX = 32;
            for (int y = 1; y < ScreenHeight - 1; y++)
            {
                Console.SetCursorPosition(separatorX, y);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("║");
                Console.ResetColor();
            }

            for (int i = -1; i < NumLanes; i++)
            {
                int x = RightPlayfieldOffset + (i * 4) + 2;
                for (int y = 1; y < ScreenHeight - 1; y++)
                {
                    Console.SetCursorPosition(x, y);
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("|");
                    Console.ResetColor();
                }
            }
        }
    }

    private void DrawUI()
    {
        Console.SetCursorPosition(2, ScreenHeight - 1);
        Console.Write(new string(' ', ScreenWidth - 4));
        Console.SetCursorPosition(2, ScreenHeight - 1);
        Console.ForegroundColor = ConsoleColor.Cyan;
        string hearts = string.Concat(Enumerable.Repeat("<3 ", _humanPlayer.Hearts));
        Console.Write($"👤PLAYER : {hearts} Score={_humanPlayer.Score}");
        
        if (_lastCollisionObject != null)
        {
            Console.Write("  ");
            _lastCollisionObject.RenderCollisionEffect(Console.CursorLeft, ScreenHeight - 1);
        }

        Console.SetCursorPosition(2, 0);
        Console.Write(new string(' ', ScreenWidth - 4));
        Console.SetCursorPosition(2, 0);
        
        var latestMessage = _messageEvents.GetAll().LastOrDefault();
        if (!string.IsNullOrEmpty(latestMessage))
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{latestMessage}");
        }
        else if (_aiPlayer != null)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            string aiHearts = string.Concat(Enumerable.Repeat("<3 ", _aiPlayer.Hearts));
            Console.Write($"BOT : {aiHearts}");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"DU SPELAR SOLO-LÄGET");
        }

        Console.ResetColor();
    }

    public bool IsGameOver()
    {
        if (_aiPlayer == null)
        {
            return !_humanPlayer.IsAlive;
        }
        
        return !_humanPlayer.IsAlive || !_aiPlayer.IsAlive;
    }

    public void ShowGameOver()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("-------------------");
        Console.WriteLine("    GAME OVER!     ");
        Console.WriteLine("-------------------");

        Console.ForegroundColor = ConsoleColor.White;

        int finalScore = _humanPlayer.Score;

        if (_aiPlayer == null)
        {
            Console.WriteLine($"Slutresultat: {finalScore} poäng!");
        }
        else if (_humanPlayer.IsAlive)
        {
            Console.WriteLine($"{_humanPlayer.Name} VINNER! (Score: {finalScore})");
        }
        else if (_aiPlayer.IsAlive)
        {
            Console.WriteLine($"{_aiPlayer.Name} VINNER! (Score: {_aiPlayer.Score})");
            Console.WriteLine($"Din poäng: {finalScore}");
        }
        else
        {
            Console.WriteLine("OAVGJORT!");
            Console.WriteLine($"Er poäng: {finalScore}");
        }

        Console.ResetColor();

        Console.WriteLine();
        Console.WriteLine("Tryck på valfri tangent för att fortsätta...");
        Console.ReadKey(true);
    }
}


