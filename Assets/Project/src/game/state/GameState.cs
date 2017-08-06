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
        public float TimeSinceLastUpdate { get; set; }

        protected GameState()
        {
            TimeSinceLastUpdate = GetUpdateFrequencyInSeconds();
        }

        public GameState Transition(GameStorage storage, GameManager manager)
        {
            TimeSinceLastUpdate = 0;
            Debug.Log("Transition from state " + this.GetType());
            var result = Evaluate(storage, manager);
            Debug.Log("To state " + result.GetType());
            return result;
        }

        public bool IsReadyToTransition(float timePassedInSeconds)
        {
            TimeSinceLastUpdate += timePassedInSeconds;
            return TimeSinceLastUpdate >= GetUpdateFrequencyInSeconds();

        }

        protected abstract GameState Evaluate(GameStorage storage, GameManager manager);
        protected virtual float GetUpdateFrequencyInSeconds()
        {
            return 10f;
        }
    }
}
