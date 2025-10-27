using HIGHWAYS.Core;
using HIGHWAYS.Movement;
using HIGHWAYS.Players;
using HIGHWAYS.Factories;
using HIGHWAYS.GameObjects;
using HIGHWAYS.Interfaces;

namespace HIGHWAYS;

/// <summary>
/// Huvudprogram - Demonstrerar användning av alla OOP2-koncept:
/// 
namespace SPACESHIP;
//TEST
/// 1. GENERICS - ObjectBuffer<T> (ObjectBuffer.cs)
///    - Används med olika typ-argument: ObjectBuffer<GameObject> (Lane.cs rad 7, 15) och ObjectBuffer<string> (Game.cs rad 24, 32)
///    - Constraint: where T : class
///    - Typsäker cirkulär buffer som fungerar med vilken referenstyp som helst
/// 
/// 2. STRATEGY PATTERN - IMovementStrategy (IMovementStrategy.cs)
///    - Subtyper: ZigZagStrategy (sicksack-rörelse) och StraightStrategy (står stilla)
///    - Injiceras i AIPlayer.cs konstruktor (rad 19, 24)
///    - Används i AIPlayer.Update() (rad 63) för att bestämma AI:ns nästa lane
///    - Användaren väljer strategi vid runtime (Program.cs rad 54-56)
/// 
/// 3. BRIDGE PATTERN - GameObject (GameObject.cs rad 12, 14)
///    - Abstraktion 1: IBehavior med 3 konkretioner (DamageBehavior, HealBehavior, ScoreBehavior)
///    - Abstraktion 2: IRenderable med 2 konkretioner (AsciiRenderer, ColoredRenderer)
///    - GameObject komponeras av båda via konstruktor (rad 16)
///    - Används i GameObject.Render() (rad 33) och HandleCollision() (rad 39)
///    - Möjliggör 6 olika kombinationer utan att skapa 6 klasser
/// 
/// 4. FACTORY METHOD - IGameObjectFactory (IGameObjectFactory.cs)
///    - Fabrik-hierarki: ObstacleFactory och PowerupFactory
///    - Produkt-hierarki: Obstacle och Powerup (båda ärver GameObject)
///    - Injiceras i Game.cs konstruktor (rad 30-32, 36-38)
///    - Används i SpawnRow() (rad 126, 133, 140) för att kapsla komplex objektskapande
/// 
/// 5. ITERATOR PATTERN - Lane (Lane.cs rad 5)
///    - Implementerar IEnumerable<GameObject>
///    - GetEnumerator() delegerar till underliggande IEnumerable (rad 55-61)
///    - Itereras i Game.CheckCollisions() (rad 175-177) och CleanupObjects() (rad 228)
///    - Kapslar ObjectBuffer och filtrerar automatiskt till aktiva objekt
/// 
/// 6. LINQ METOD-SYNTAX (Game.cs)
///    - CheckCollisions() (rad 175-178): .Where().Where().ToList() för kollisionsdetektion
///    - CleanupObjects() (rad 227-239): .SelectMany().Count() och .OfType<T>() för statistik
///    - DrawUI() (rad 364): .LastOrDefault() för att hämta senaste meddelande
///    - Simplifierar nested loops, filtrering och typ-kontroller
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8; // För att kunna pimpa spelet lite
        
        while (true)
        {
            ShowWelcomeScreen();

            Console.WriteLine("Undvik hindren och plocka powerups! Blå= Dubbla score. Grön= Extra HP!");
            Console.WriteLine("\nVälj gamemode:");
            Console.WriteLine("\n1. Solo ");
            Console.WriteLine("2. Mot Bot ");
            Console.WriteLine("3. Avsluta");
            Console.Write("\nVälj (1-3): ");
            
            var choice = Console.ReadKey(true).KeyChar;
            Console.WriteLine();
            
            if (choice == '3')
            {
                return;
            }
            
            var humanPlayer = new HumanPlayer("Spelare", startLane: 2, maxLanes: 5);
            
            AIPlayer? aiPlayer = null;
            if (choice == '2')
            {
                Console.WriteLine("\nVälj bot-strategy:");
                Console.WriteLine("\n1. ZigZag (svår)");
                Console.WriteLine("2. Straight (lätt)");
                Console.Write("\nVal (1-2): ");
                
                var strategyChoice = Console.ReadKey(true).KeyChar;
                Console.WriteLine();
                
                IMovementStrategy aiStrategy = strategyChoice == '2' 
                    ? new StraightStrategy() 
                    : new ZigZagStrategy();
                
                aiPlayer = new AIPlayer("Bot", startLane: 2, maxLanes: 5, aiStrategy);
                Console.WriteLine("BOTMODE!");
            }
            else
            {
                Console.WriteLine("SOLOMODE!");
            }

            Console.WriteLine("\nTryck på valfri tangent för att starta spelet...");
            Console.ReadKey(true);

            // skapar och injicerar fabriker via dependency injection istället för hårdkodning
            var obstacleFactory = new ObstacleFactory();
            var healthPowerupFactory = new PowerupFactory(PowerupType.Health);
            var scorePowerupFactory = new PowerupFactory(PowerupType.Score);

            var game = new Game(humanPlayer, aiPlayer, obstacleFactory, healthPowerupFactory, scorePowerupFactory);
            var gameLoop = new GameLoop(game);

            gameLoop.Start();
            
        }
    }

    static void ShowWelcomeScreen()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔══════════════════════════════════════════════════════╗");
        Console.WriteLine("║                                                      ║");
        Console.WriteLine("║                        HIGHWAY!                      ║");
        Console.WriteLine("║                                                      ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════╝");
        Console.ResetColor();
    }
}

