using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Traitorstown.src.model
{
    public class Game
    {
        public int Id { get; set; }
        public GameStatus status;

        public Game(int id, GameStatus status)
        {
            Id = id;
            this.status = status;
        }
    }
}

