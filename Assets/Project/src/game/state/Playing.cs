using System;
using Traitorstown.src.model;

namespace Traitorstown.src.game.state
{
    public class Playing : GameState
    {
        private int turn;

        protected override GameState Evaluate(GameStorage storage, GameManager manager)
        {
            if (storage.Game.Status == GameStatus.FINISHED)
            {
                return new MainMenu();
            }

            if (turn != storage.Game.Turn)
            {
                manager.GetCards();
                turn = storage.Game.Turn;
                return this;
            }

            manager.GetCurrentGame();
            return this;
        }

        protected override float GetUpdateFrequencyInSeconds()
        {
            return 5f;
        }
    }
}