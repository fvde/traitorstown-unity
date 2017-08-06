using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traitorstown.src.model;

namespace Traitorstown.src.game.state
{
    public class StartingGame : GameState
    {
        private bool triedToRestoreGame;

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
            if (storage.PlayerId.HasValue && !triedToRestoreGame)
            {
                manager.GetCurrentGame();
                triedToRestoreGame = true;
                return this;
            }

            // Continue current game
            if (storage.Game != null)
            {
                if (storage.Game.Status == GameStatus.OPEN) return new Lobby();
                if (storage.Game.Status == GameStatus.PLAYING) return new Playing();
                if (storage.Game.Status == GameStatus.FINISHED) return new MainMenu();
            }

            return new MainMenu();
        }

        protected override float GetUpdateFrequencyInSeconds()
        {
            return 1f;
        }
    }
}
