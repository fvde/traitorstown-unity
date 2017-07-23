using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Traitorstown.src.model
{
    public class Player
    {
        public int Id { get; }
        public bool Ready { get; }
        public List<Resource> Resources;

        public Player(int id, bool ready, List<Resource> resources)
        {
            Id = id;
            Ready = ready;
            Resources = resources;
        }
    }
}
