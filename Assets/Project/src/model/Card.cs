using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Traitorstown.src.model
{
    public class Card
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Card(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}

