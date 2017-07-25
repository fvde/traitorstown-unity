using System;

namespace Traitorstown.src.game.state
{
    public class Playing : GameState
    {
        protected override GameState Evaluate(GameStorage storage, GameManager manager)
        {
            return this;
        }
    }
}