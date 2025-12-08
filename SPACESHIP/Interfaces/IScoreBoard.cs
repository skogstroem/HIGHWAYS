using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIGHWAYS.Interfaces
{
    public interface IScoreBoard
    {
        void Draw();

        void Save(string playerName, int score, int gameMode);
    }
}
