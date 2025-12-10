using System;
using System.Linq;
using HIGHWAYS.FileManager;
using HIGHWAYS.Interfaces;

namespace HIGHWAYS
{
    public class AdvancedScoreBoard : ScoreBoard
    {

        public AdvancedScoreBoard(IFile storage)
        {
            _file = storage;
        }

        public override void Draw()
        {
            var entries = _file.Fetch();

            if (entries.Count == 0)
            {
                Console.WriteLine("Inga sparade resultat ännu.");
                return;
            }

            Console.WriteLine("ADVANCED SCOREBOARD");

            var grouped = entries
                .GroupBy(e => e.Difficulty)
                .OrderBy(g => g.Key);

            foreach (var group in grouped)
            {

                string gameMode;
                
                switch (group.Key)
                {
                    case 0:
                        gameMode = "Solo";
                        break;
                    case 1:
                        gameMode = "ZigZag";
                        break;
                    case 2:
                        gameMode = "Straight Strategy";
                        break;
                    case 3:
                        gameMode = "Random Strategy";
                        break;
                    default:
                        gameMode = "Unknown";
                        break;
                }
                
                Console.WriteLine($"\nGamemode {gameMode}:");

                var players = group
                    .OrderByDescending(e => e.Score)
                    .Take(4)
                    .ToList();

                // Pad till 5 rader med "tomt" om färre än 5 poster finns.

                int position = 1;
                foreach (var player in players)
                    
                {
                    Console.WriteLine($"  {position}. {player.Score} p {player.Name}");
                    position++;
                }
            }
        }
    }
}

