using HIGHWAYS.Core;
using HIGHWAYS.Players;
using HIGHWAYS.Factories;
using HIGHWAYS.FileManager;
using HIGHWAYS.Interfaces;

namespace HIGHWAYS;

class Program
{
    static void Main(string[] args)
    {
        GameLogic gameLogic = new GameLogic();
        IFile file = gameLogic.SelectFileType(); // Här hämtar vi filen
        
        while (true)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; // För att kunna pimpa spelet lite!
            Console.CursorVisible = false;

            gameLogic.ShowWelcomeScreen();
            gameLogic.ShowStartMenu();
            
            ScoreBoard scoreBoard = new NormalScoreBoard(file);
            Player humanPlayer = new Player("Spelare", startLane: 2, maxLanes: 5);
            AIPlayer aiPlayer = gameLogic.MenuSeletion(file, humanPlayer, scoreBoard);
           
            gameLogic.StartMessage();

            // Skapar och injicerar fabriker via dependency injection istället för hårdkodning
            IGameObjectFactory obstacleFactory = new ObstacleFactory();
            IGameObjectFactory powerupFactory = new PowerupFactory();

            Game game = new Game(humanPlayer, aiPlayer, obstacleFactory, powerupFactory, scoreBoard); // Skicka in filen här till våran scoreboard 
            Loop gameLoop = new Loop(game);

            gameLoop.Start();
        }
    }
}

