using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIGHWAYS.FileManager;

namespace HIGHWAYS.Interfaces
{
    public abstract class ScoreBoard
    {

        protected IFile _file;
        
        public virtual void Draw() {}

        public void Save(IPlayer player)
        {
            _file.Save(player);
        }
    }
}
