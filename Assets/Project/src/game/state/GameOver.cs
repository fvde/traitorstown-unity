using System;
using Traitorstown.src.model;

namespace Traitorstown.src.game.state
{
    public class GameOver : GameState
    {
        protected override GameState Evaluate(GameStorage storage, GameManager manager)
        {
            manager.Reset();
            return new MainMenu();
        }
    }
}