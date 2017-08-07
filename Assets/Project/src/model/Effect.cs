using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Traitorstown.src.model
{
    public class Effect
    {
        public string Name { get; }
        public int RemainingTurns { get; }

        public Effect(string name, int remainingTurns)
        {
            Name = name;
            RemainingTurns = remainingTurns;
        }
    }
}

