using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Traitorstown.src.model
{
    public class Card
    {
        public int Id { get; }
        public string Name { get; }

        public Card(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public Card()
        {

        }
    }
}

