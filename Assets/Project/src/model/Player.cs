using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Traitorstown.src.model
{
    public class Player
    {
        public int Id { get; }
        public bool Ready { get; }

        public Player(int id, bool ready)
        {
            Id = id;
            Ready = ready;
        }
    }
}
