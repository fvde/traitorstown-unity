using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Traitorstown.src.game.state
{
    public abstract class GameState
    {
        public GameState Transition(GameStorage storage, GameManager manager)
        {
            Debug.Log("Transition from state " + this.GetType());
            var result = Evaluate(storage, manager);
            Debug.Log("To state " + result.GetType());
            return result;
        }

        protected abstract GameState Evaluate(GameStorage storage, GameManager manager);
    }
}
