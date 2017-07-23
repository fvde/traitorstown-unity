using System;
using System.Collections;
using System.Collections.Generic;
using Traitorstown.src.model;
using UnityEngine;

namespace Traitorstown.src.http.representation
{
    [Serializable]
    public class TurnRepresentation
    {
        public int counter;

        public TurnRepresentation(int counter)
        {
            this.counter = counter;
        }

        public Turn toTurn()
        {
            return new Turn(counter);
        }
    }
}

