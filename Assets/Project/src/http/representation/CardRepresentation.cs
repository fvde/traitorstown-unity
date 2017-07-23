using System;
using System.Collections;
using System.Collections.Generic;
using Traitorstown.src.model;
using UnityEngine;

namespace Traitorstown.src.http.representation
{
    [Serializable]
    public class CardRepresentation
    {
        public long id;
        public string name;

        public CardRepresentation(long id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}

