using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Traitorstown.src.model
{
    public class Player : TraitorsTownObject
    {
        public int Id { get; set; }

        public Player(int id)
        {
            Id = id;
        }
    }
}
