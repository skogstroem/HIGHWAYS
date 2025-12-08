using HIGHWAYS.Interfaces;
using HIGHWAYS.Movement;
using HIGHWAYS.Players;
namespace HIGHWAYS;

public class GameLogic
{

    public void ShowWelcomeScreen()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔══════════════════════════════════════════════════════╗");
        Console.WriteLine("║                                                      ║");
        Console.WriteLine("║                        HIGHWAYS!                     ║");
        Console.WriteLine("║                                                      ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════╝");
        Console.ResetColor();
    }

    public void ShowSoloMode()
    {
        Console.WriteLine("SOLOMODE!");
    }

    public void PlayVersusBot()
    {
        Console.WriteLine("\nVälj bot-strategy:" +
                          "\n 1. ZigZag (datorn rör sig smart)" +
                          "\n 2. Straight (datorn åker bara rakt fram)" +
                          "\n 3. Random strategy (datorn rör sig spontant" +
                          "\n Val (1-3);"
        );
    }

    public void EndProgram()
    {
        Console.Clear();
        Console.WriteLine("Spelet avslutat! Tack för att du har spelat :-)");
        Environment.Exit(1);
    }
    public void ShowScoreboard()
    {
        Console.Clear();
        var storage = new MockFile();
        var board = new NormalScoreBoard(storage);
        board.Draw();
        Console.WriteLine("\n Tryck på en knapp för att återgå till startmenyn.");
        Console.ReadKey();
    }
    public void ShowAdvancedScoreboard()
    {
        Console.Clear();
        var storage = new MockFile();
        var board = new AdvancedScoreBoard(storage);
        board.Draw();
        Console.WriteLine("\n Tryck på en knapp för att återgå till startmenyn.");
        Console.ReadKey();
    }

    public void ShowStartMenu()
    {
        Console.Write(" Undvik hindren och plocka powerups! \n\n" +
                      " Powerups: Grön = Extra HP \n " +
                      "Blå = Dubbla score! \n " +
                      "Röd = BOMB! GAME OVER! \n\n " +
                      "Välj gamemode: \n " +
                      "1. Solo: \n " +
                      "2. Mot bot: \n " +
                      "3. Avsluta spelet \n\n " +
                      "4. Visa Scoreboard \n " +
                      "5. Visa Advanced Scoreboard \n\n " +
                      "Gör ditt val (1-5):"
        );
    }

    public AIPlayer MenuSeletion()
    {
        while (true)
        {
            var key = Console.ReadKey(true).KeyChar;
            if (int.TryParse(key.ToString(), out var choice) && choice is >= 1 and <= 5)
            {
                switch (choice)
                {
                    case 1:
                        ShowSoloMode();
                        return null;
                    case 2:
                        PlayVersusBot();
                        IStrategy selectedStrategy = SelectStrategyAndCreateBot();
                        return new AIPlayer("Bot", startLane: 2, maxLanes: 5, selectedStrategy);
                    case 3:
                        EndProgram();
                        break;
                    case 4:
                        ShowScoreboard();
                        Console.Clear();
                        ShowStartMenu();
                        break;
                    case 5:
                        ShowAdvancedScoreboard();
                        Console.Clear();
                        ShowStartMenu();
                        break;
                }
            }
            else
            {
                Console.Write("\n Ogiltigt val! Vänligen välj mellan: "
                              + "\n 1. Solo"
                              + "\n 2. Mot bot"
                              + "\n 3. Avsluta spelet"
                              + "\n 4. Visa Scoreboard"
                              + "\n 5. Visa Advanced Scoreboard");
            }
        }
    }

    public void StartMessage()
    {
        Console.WriteLine("\nTryck på valfri tangent för att starta spelet...");
        Console.ReadKey(true);
    }


    public IStrategy SelectStrategyAndCreateBot()
    {
        bool validChoice = false;
        int choice = 1;

        while (!validChoice)
        {
            var keyStroke = Console.ReadKey(true).KeyChar;
            if (int.TryParse(keyStroke.ToString(), out choice) && choice <= 3 && choice >= 1)
            {
                validChoice = true;
            }
            else
            {
                Console.Write("\n Ogiltigt val! Vänligen välj mellan: " +
                              "\n 1. ZigZag (datorn rör sig smart)" +
                              "\n 2. Straight (datorn åker bara rakt fram)" +
                              "\n 3. Random strategy (datorn rör sig spontant"
                              );
            }
        }

        IStrategy selectedStrategy = choice switch
        {
            1 => new ZigZagStrategy(),
            2 => new StraightStrategy(),
            3 => new AdvancedStrategy(),
            _ => new StraightStrategy()
        };

        return selectedStrategy;
    }
}