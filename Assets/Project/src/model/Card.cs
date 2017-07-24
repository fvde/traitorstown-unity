using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Traitorstown.src.model
{
    public class Card
    {
        public int Id { get; }
        public string Name { get; }
        public string Description { get; }
        public List<Resource> Costs { get; }

        public Card(int id, string name, string description, List<Resource> costs)
        {
            Id = id;
            Name = name;
            Description = description;
            Costs = costs;
        }

        public override int GetHashCode()
        {
            if (Name == null) return 0;
            return Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            Card other = obj as Card;
            return other != null && Id.Equals(other.Id);
        }
    }
}

