using System;

namespace Traitorstown.src.game.state
{
    public class Lobby : GameState
    {
        private DateTime lastCheckForReadyGame = DateTime.Now;

        protected override GameState Evaluate(GameStorage storage, GameManager manager)
        {
            // left game
            if (!storage.GameId.HasValue)
            {
                return new MainMenu();
            }

            // start playing
            if (storage.TurnCounter.HasValue)
            {
                return new Playing();
            }
            
            // check if game started
            if ((DateTime.Now - lastCheckForReadyGame).Seconds > 5)
            {
                lastCheckForReadyGame = DateTime.Now;
                manager.GetCurrentGame();
            }

            return this;
        }
    }
}