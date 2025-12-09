using System;
using System.Linq;
using HIGHWAYS.FileManager;
using HIGHWAYS.Interfaces;

namespace HIGHWAYS
{
    public class AdvancedScoreBoard : ScoreBoard
    {
        private readonly IFile _storage;

        public AdvancedScoreBoard(IFile storage)
        {
            _storage = storage;
        }

        public override void Draw()
        {
            var entries = _storage.Fetch();

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
                Console.WriteLine($"\nGamemode {group.Key}:");

                var players = group
                    .OrderByDescending(e => e.Score)
                    .Take(4)
                    .ToList();

                // Pad till 5 rader med "tomt" om färre än 5 poster finns.

                int position = 1;
                foreach (var player in players)
                    
                {
                    Console.WriteLine($"  {position}. {player.Score}p {player.Name}, {player.Difficulty}");
                    position++;
                }
            }
        }
    }
}

