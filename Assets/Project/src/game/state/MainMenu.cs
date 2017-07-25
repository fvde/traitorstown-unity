using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Traitorstown.src.game.state
{
    public class MainMenu : GameState
    {
        protected override GameState Evaluate(GameStorage storage, GameManager manager)
        {
            return this;
        }
    }
}
