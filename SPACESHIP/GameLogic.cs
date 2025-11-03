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

    public void ShowStartMenu()
    {
        Console.Write(" Undvik hindren och plocka powerups! \n\n" +
                      " Powerups: Grön = Extra HP \n " +
                      "Blå = Dubbla score! \n " +
                      "Röd = BOMB! GAME OVER! \n\n " +
                      "Välj gamemode: \n " +
                      "1. Solo: \n " +
                      "2. Mot bot: \n "  +
                      "3. Avsluta spelet \n\n " +
                      "Välj (1-3):"
        );
    }

    public AIPlayer MenuSeletion ()
    {
        bool validChoice = false;
        int choice = 1;
        
        AIPlayer? aiPlayer = null;
        
        while (!validChoice)
        {
            var key = Console.ReadKey(true).KeyChar;
            if (int.TryParse(key.ToString(), out choice) && choice <= 3 && choice >= 1)
            {
                validChoice = true;
            }
            else
            {
                Console.Write("\n Ogiltigt val! Vänligen välj mellan: " 
                              + "\n 1. Solo" +
                              "\n 2. Mot bot" + 
                              "\n 3. Avsluta spelet");
            } 
        }
        
        switch (choice)
        {
            case 1:
                ShowSoloMode();
                break;

            case 2:
                PlayVersusBot();
                IStrategy selectedStrategy = SelectStrategyAndCreateBot();
                aiPlayer = new AIPlayer("Bot", startLane: 2, maxLanes: 5, selectedStrategy);
                break;

            case 3:
                EndProgram();
                break;
        }

        return aiPlayer;
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