using System;
using System.Collections;
using System.Collections.Generic;
using Traitorstown.src.model;
using UnityEngine;

namespace Traitorstown.src.http.representation
{
    [Serializable]
    public class PlayerRepresentation
    {
        public long id;
        public bool ready;

        public PlayerRepresentation(long id, bool ready)
        {
            this.id = id;
            this.ready = ready;
        }

        public Player ToPlayer()
        {
            return new Player((int)id, ready);
        }
    }
}

