using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Traitorstown.src.game.state
{
    public class LookingForGame : GameState
    {
        private Boolean lookedForGame = false;

        protected override GameState Evaluate(GameStorage storage, GameManager manager)
        {
            if (storage.GameId.HasValue)
            {
                return new Lobby();
            }

            if (storage.OpenGames.Count == 0 && !lookedForGame)
            {
                manager.GetOpenGames();
                lookedForGame = true;
                return this;
            }
            else if (storage.OpenGames.Count == 0 && lookedForGame)
            {
                manager.CreateNewGame();
                return this;
            }
            else
            {
                manager.JoinGame(storage.OpenGames[0].Id);
                return this;
            }
        }

        protected override float GetUpdateFrequencyInSeconds()
        {
            return 2f;
        }
    }
}
