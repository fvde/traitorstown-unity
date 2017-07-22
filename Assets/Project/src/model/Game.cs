using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Traitorstown.src.model
{
    public class Game : TraitorsTownObject
    {
        public long Id { get; set; }

        public GameStatus status;

        public Game(long id, GameStatus status)
        {
            Id = id;
            this.status = status;
        }
    }
}

