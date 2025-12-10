using System;
using System.Linq;
using HIGHWAYS.FileManager;
using HIGHWAYS.Interfaces;

namespace HIGHWAYS
{
    internal class NormalScoreBoard : ScoreBoard
    {

        public NormalScoreBoard (IFile storage)
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

            Console.WriteLine("NORMAL SCOREBOARD \n");

            var players = entries
                .OrderByDescending(e => e.Score)
                .ToList();

            int position = 1;
            
            foreach (var player in players)
            {
                Console.WriteLine($"{position}. {player.Score}p, {player.Name} Gamemode: {player.Difficulty}");
                position++;
            }
        }
    }
}

