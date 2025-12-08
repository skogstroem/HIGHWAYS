using System.Collections.Generic;

namespace HIGHWAYS.Interfaces
{
    interface IFile
    {
        void Save(string playerName, int score, int gameMode);
        List<(string PlayerName, int Score, int GameMode)> Fetch();
    }

    // Mock IFile där vi får för att få fyra rader data
    class MockFile : IFile
    {
        private readonly List<(string PlayerName, int Score, int GameMode)> _entries =
        [
            ("Anna", 1200, 0),
            ("Bertil", 950, 1),
            ("Cesar", 1500, 2),
            ("Doris", 800, 3)
        ];

        public void Save(string playerName, int score, int gameMode)
        {
            _entries.Add((playerName, score, gameMode));
        }

        public List<(string PlayerName, int Score, int GameMode)> Fetch()
        {
            return new List<(string PlayerName, int Score, int GameMode)>(_entries);
        }
    }
}

