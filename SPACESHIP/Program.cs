using HIGHWAYS.Core;
using HIGHWAYS.Players;
using HIGHWAYS.Factories;
using HIGHWAYS.Interfaces;

namespace HIGHWAYS;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; // För att kunna pimpa spelet lite
            Console.CursorVisible = false;
            GameLogic gameLogic = new GameLogic();

            gameLogic.ShowWelcomeScreen();
            gameLogic.ShowStartMenu();

            Player humanPlayer = new Player("Spelare", startLane: 2, maxLanes: 5);
            AIPlayer aiPlayer = gameLogic.MenuSeletion();
           
            gameLogic.StartMessage();

            // Skapar och injicerar fabriker via dependency injection istället för hårdkodning
            IGameObjectFactory obstacleFactory = new ObstacleFactory();
            IGameObjectFactory powerupFactory = new PowerupFactory();

            Game game = new Game(humanPlayer, aiPlayer, obstacleFactory, powerupFactory);
            Loop gameLoop = new Loop(game);

            gameLoop.Start();
        }
    }
}

