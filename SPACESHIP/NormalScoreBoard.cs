using System;
using System.Linq;
using HIGHWAYS.Interfaces;

namespace HIGHWAYS
{
    internal class NormalScoreBoard : IScoreBoard
    {
        private readonly IFile _storage;

        public NormalScoreBoard(IFile storage)
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

            Console.WriteLine("NORMAL SCOREBOARD \n");

            var ordered = entries
                .OrderByDescending(e => e.Score)
                .ToList();

            int position = 1;
            foreach (var entry in ordered)
            {
                Console.WriteLine($"{position}. {entry.Score}p {entry.PlayerName} (Gamemode: {entry.GameMode})");
                position++;
            }
        }
    }
}

