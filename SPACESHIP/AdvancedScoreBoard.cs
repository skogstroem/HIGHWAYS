using System;
using System.Linq;
using HIGHWAYS.Interfaces;

namespace HIGHWAYS
{
    public class AdvancedScoreBoard : IScoreBoard
    {
        private readonly IFile _storage;

        public AdvancedScoreBoard(IFile storage)
        {
            _storage = storage;
        }

        public void Save(string playerName, int score, int gameMode)
        {
            _storage.Save(playerName, score, gameMode);
        }

        public void Draw()
        {
            var entries = _storage.Fetch();

            if (entries.Count == 0)
            {
                Console.WriteLine("Inga sparade resultat ännu.");
                return;
            }

            Console.WriteLine("ADVANCED SCOREBOARD");

            var grouped = entries
                .GroupBy(e => e.GameMode)
                .OrderBy(g => g.Key);

            foreach (var group in grouped)
            {
                Console.WriteLine($"\nGamemode {group.Key}:");

                var top = group
                    .OrderByDescending(e => e.Score)
                    .Take(4)
                    .ToList();

                // Pad till 5 rader med "tomt" om färre än 5 poster finns.
                while (top.Count < 4)
                {
                    top.Add(("---", 0, group.Key));
                }

                int position = 1;
                foreach (var entry in top)
                {
                    Console.WriteLine($"  {position}. {entry.Score}p {entry.PlayerName}");
                    position++;
                }
            }
        }
    }
}

