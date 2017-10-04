using System;
using Traitorstown.src.model;

namespace Traitorstown.src.game.state
{
    public class Lobby : GameState
    {
        protected override GameState Evaluate(GameStorage storage, GameManager manager)
        {
            // left game
            if (storage.Game == null)
            {
                return new MainMenu();
            }

            // start playing
            if (storage.Game != null && storage.Game.Status == GameStatus.PLAYING)
            {
                return new Playing();
            }

            // check if game started
            manager.GetCurrentGame();
            return this;
        }

        protected override float GetUpdateFrequencyInSeconds()
        {
            return 1f;
        }
    }
}