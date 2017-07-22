using System;
using System.Collections;
using System.Collections.Generic;
using Traitorstown.src.model;
using UnityEngine;

namespace Traitorstown.src.http.representation
{
    [Serializable]
    public class GameRepresentation
    {
        public long id;
        public GameStatus status;

        public GameRepresentation(long id, GameStatus status)
        {
            this.id = id;
            this.status = status;
        }
    }
}

