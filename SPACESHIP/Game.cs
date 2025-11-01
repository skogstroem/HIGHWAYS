using HIGHWAYS.GameObjects;
using HIGHWAYS.Players;
using HIGHWAYS.Generics;
using HIGHWAYS.Interfaces;

namespace HIGHWAYS.Core;

public class Game
{
    private const int NumberOfLanes = 5;

    private const int GameWidth = 80;
    private const int GameHeight = 30;
    private const int PlayersY = 27; //Y-position för spelare används för kollisionsdet

    private const int LaneOffset = 5; //startar spelplanen 5positioner till höger
    private const int LaneOffsetAI = 42; //start för AI-planen

    private readonly List<Lane> _playerLanes;
    private readonly List<Lane> _aiLanes;

    private readonly Player _humanPlayer;
    private readonly AIPlayer? _aiPlayer;

    private readonly IGameObjectFactory _obstacleFactory;
    private readonly IGameObjectFactory _powerupFactory;

    private readonly Random _random;

    private readonly ObjectBuffer<string> _messageEvents;
    private GameObject? _lastCollisionObject;


    private int _rowCounter; //räknare för spawning
    private const int RowsPerSpawn = 5; //antal rader mellan spawn av objekt

    public Game(Player humanPlayer, AIPlayer? aiPlayer,IGameObjectFactory obstacleFactory,IGameObjectFactory powerupFactory)
    {
        _humanPlayer = humanPlayer;
        _aiPlayer = aiPlayer;
        _obstacleFactory = obstacleFactory;
        _powerupFactory = powerupFactory;
        _messageEvents = new ObjectBuffer<string>(100);

        _playerLanes = new List<Lane>();

        for (int i = 0; i < NumberOfLanes; i++)
        {
            int xPos = LaneOffset + (i * 4); //skapar och lagrar alla lanes xPos för själva lane-OBJEKTEN i _playerLanes
            _playerLanes.Add(new Lane(i, xPos));
        }

        _aiLanes = new List<Lane>();
        if (_aiPlayer != null)
        {
            for (int i = 0; i < NumberOfLanes; i++)
            {
                int xPos = LaneOffsetAI + (i * 4);
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
            lane.UpdateAll(); //uppdatera (öka Y, för) alla objekt i varje lane
        }

        if (_aiPlayer != null)
        {
            foreach (var lane in _aiLanes)
            {
                lane.UpdateAll();
            }
        }

        SpawnObjects(); //metod som ökar rowCounter och spawnar objektrad if...
        CheckCollisions(); //jämför spelare och objekts Y för ev kollision
        CleanupObjects(); //tar bort inaktiva objekt och objekt utanför skärmen
        _humanPlayer.IncreaseScore(1); //poängcounters
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
        int obstacle1Lane = _random.Next(0, NumberOfLanes);
        int obstacle2Lane = _random.Next(0, NumberOfLanes);
        
        //säkerställer att spawn sker på olika lanes
        while (obstacle2Lane == obstacle1Lane)
        {
            obstacle2Lane = _random.Next(0, NumberOfLanes);
        }

        // skapa själva hindret i ovan randomiserade lanesen för spelaren
        var obj1 = _obstacleFactory.CreateGameObject(_playerLanes[obstacle1Lane].XPosition, 0);
        _playerLanes[obstacle1Lane].AddObject(obj1);
        
        var obj2 = _obstacleFactory.CreateGameObject(_playerLanes[obstacle2Lane].XPosition, 0);
        _playerLanes[obstacle2Lane].AddObject(obj2);

        //därefter, powerup spawn med exakt samma logik

        // 25% chans för powerup
        if (_random.Next(0, 100) < 25)
        {
            int powerupLane = _random.Next(0, NumberOfLanes);
            
            while (powerupLane == obstacle1Lane || powerupLane == obstacle2Lane)
            {
                powerupLane = _random.Next(0, NumberOfLanes);
            }
            
            var powerup = _powerupFactory.CreateGameObject(_playerLanes[powerupLane].XPosition, 0);
            _playerLanes[powerupLane].AddObject(powerup);
        }

        // Om det finns AI, gör samma sak för AI:n
        if (_aiPlayer != null)
        {
            var aiObj1 = _obstacleFactory.CreateGameObject(_aiLanes[obstacle1Lane].XPosition, 0);
            _aiLanes[obstacle1Lane].AddObject(aiObj1);
            
            var aiObj2 = _obstacleFactory.CreateGameObject(_aiLanes[obstacle2Lane].XPosition, 0);
            _aiLanes[obstacle2Lane].AddObject(aiObj2);
        }
    }

    private void CheckCollisions()
    {
        var humanLane = _playerLanes[_humanPlayer.CurrentLane];
        
        foreach (var obj in humanLane)
        {
            if (!obj.IsActive)
                continue;
                
            int distance = Math.Abs(obj.Y - PlayersY);

            if (distance == 0)
            {
                // kollisionseffekt längst upp
                if (obj is Powerup)
                {
                    _messageEvents.TryAdd($"       LETS GO!");
                }
                else if (obj is Obstacle obstacle && obstacle.Type != ObstacleType.Bomb)
                {
                    _messageEvents.TryAdd("         AUCH!");
                }
                
                _lastCollisionObject = obj;
                obj.HandleCollision(_humanPlayer);
            }
        }

        // samma för AI om det finns
        if (_aiPlayer != null)
        {
            var aiLane = _aiLanes[_aiPlayer.CurrentLane];
            
            foreach (var obj in aiLane)
            {
                if (!obj.IsActive)
                    continue;
                    
                int distance = Math.Abs(obj.Y - PlayersY);
                if (distance == 0)
                {
                    obj.HandleCollision(_aiPlayer);
                }
            }
        }
    }

    private void CleanupObjects()
    {
        // såfort objekts Y är mindre än -1 så är de irrelevanta och tas bort.
        foreach (var lane in _playerLanes)
        {
            lane.RemoveInactiveObjects();
            lane.RemoveWhere(obj => obj.Y <= -1);
        }

        if (_aiPlayer != null)
        {
            foreach (var lane in _aiLanes)
            {
                lane.RemoveInactiveObjects();
                lane.RemoveWhere(obj => obj.Y <= -1);
            }
        }
    }

    public void DrawInitialFrame()
    { 
        DrawLanes(); // dags att rita ut själva lanesen
    }

    public void Render()
    {
        ClearPlayArea(); //tömmer alla lanes

        foreach (var lane in _playerLanes)
        {
            lane.RenderAll(); //fyller alla lanes igen
        }

        _humanPlayer.Render(PlayersY); //fyller i spelaren

        if (_aiPlayer != null)
        {
            foreach (var lane in _aiLanes)
            {
                lane.RenderAll();
            }
            
            _aiPlayer.Render(PlayersY);
        }

        DrawUI(); //ritar UI längst upp och längst ner
    }

    private void ClearPlayArea() //rensningmetoden
    {
        for (int laneNum = 0; laneNum < NumberOfLanes; laneNum++)
        {
            int laneX = LaneOffset + (laneNum * 4);
            for (int y = 1; y < GameHeight - 1; y++)
            {
                Console.SetCursorPosition(laneX, y);
                Console.Write("  ");
            }
        }

        if (_aiPlayer != null)
        {
            for (int laneNum = 0; laneNum < NumberOfLanes; laneNum++)
            {
                int laneX = LaneOffsetAI + (laneNum * 4);
                for (int y = 1; y < GameHeight - 1; y++)
                {
                    Console.SetCursorPosition(laneX, y);
                    Console.Write("  ");
                }
            }
        }
    }

    private void DrawLanes() //ritar själva lanes
    {
        for (int i = -1; i < NumberOfLanes; i++)
        {
            int x = LaneOffset + (i * 4) + 2;
            for (int y = 1; y < GameHeight - 1; y++)
            {
                Console.SetCursorPosition(x, y);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("|");
                Console.ResetColor();
            }
        }

        if (_aiPlayer != null) //ritar divider och lanes för AI if...
        {
            int separatorX = 32;
            for (int y = 1; y < GameHeight - 1; y++)
            {
                Console.SetCursorPosition(separatorX, y);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("║");
                Console.ResetColor();
            }

            for (int i = -1; i < NumberOfLanes; i++)
            {
                int x = LaneOffsetAI + (i * 4) + 2;
                for (int y = 1; y < GameHeight - 1; y++)
                {
                    Console.SetCursorPosition(x, y);
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("|");
                    Console.ResetColor();
                }
            }
        }
    }

    private void DrawUI()  // Rita info "utanför" spelplanen
    {
        string hearts = "";
        for (int i = 0; i < _humanPlayer.Hearts; i++)
        {
            hearts += "<3 ";
        }

        //för att rita ut rensar vi först UIn för konsekvent visning
        Console.SetCursorPosition(2, GameHeight - 1);//uppe
        Console.Write(new string(' ', GameWidth - 4));

        Console.SetCursorPosition(2, 0);//nere
        Console.Write(new string(' ', GameWidth - 4));

        //placerar cursor och skriver längst ner
        Console.SetCursorPosition(2, GameHeight - 1);
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($"PLAYER 1 : {hearts} Score={_humanPlayer.Score}");

        if (_lastCollisionObject != null)
        {
            Console.Write("  ");
            _lastCollisionObject.RenderCollisionEffect(Console.CursorLeft, GameHeight - 1);
        }

        // placerar cursor längst upp för att skriva längst upp
        Console.SetCursorPosition(2, 0);
        
        string[] messages = _messageEvents.GetAll().ToArray();

        if (messages.Length > 0)
        {
            string latestMessage = messages[messages.Length - 1];
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(latestMessage);
        }
        else if (_aiPlayer != null)
        {
            // Visa AI info
            Console.ForegroundColor = ConsoleColor.Yellow;
            string aiHearts = "";
            for (int i = 0; i < _aiPlayer.Hearts; i++)
            {
                aiHearts += "<3 ";
            }
            Console.Write($"BOT : {aiHearts}");
        }
        else
        {
            // visar ett initiellt meddelande tills event
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"DU SPELAR SOLO-LÄGET");
        }

        Console.ResetColor();
    }

    public bool IsGameOver() // metod för om död

    {
        if (!_humanPlayer.IsAlive)
            return true;
            
        if (_aiPlayer != null && !_aiPlayer.IsAlive)
            return true;
            
        return false;
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


