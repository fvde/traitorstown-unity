using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Traitorstown.src.game.state
{
    public class StartingGame : GameState
    {
        protected override GameState Evaluate(GameStorage storage, GameManager manager)
        {
            // Register
            if (String.IsNullOrEmpty(storage.UserName))
            {
                manager.Register("test");
                return this;
            }

            // Login
            if (!String.IsNullOrEmpty(storage.UserName) && storage.PlayerId == null)
            {
                manager.Login("test");
                return this;
            }

            // Restore current game
            if (storage.PlayerId.HasValue && !storage.TurnCounter.HasValue)
            {
                manager.GetCurrentGame();
                return this;
            }

            // Continue current game
            if (storage.GameId.HasValue && storage.PlayerId.HasValue && storage.TurnCounter.HasValue)
            {
                return new Playing();
            }

            return new MainMenu();
        }
    }
}
