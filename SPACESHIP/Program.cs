using HIGHWAYS.Core;
using HIGHWAYS.Movement;
using HIGHWAYS.Players;
using HIGHWAYS.Factories;
using HIGHWAYS.GameObjects;
using HIGHWAYS.Interfaces;

namespace HIGHWAYS;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8; // För att kunna pimpa spelet lite
        
        while (true)
        {
            ShowWelcomeScreen();

            Console.WriteLine("Undvik hindren och plocka powerups!");
            Console.WriteLine("Powerups: Grön = Extra HP // Blå = Dubbla score!");
            Console.WriteLine("Hinder: Grå = Förlora HP // Röd = Game over!");
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
            var powerupFactory = new PowerupFactory();

            var game = new Game(humanPlayer, aiPlayer, obstacleFactory, powerupFactory);
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

