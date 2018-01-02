using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Traitorstown.src.model
{
    [Serializable]
    public enum GameStatus
    {
        OPEN,
        PLAYING,
        FINISHED,
        CANCELLED
    }
}
