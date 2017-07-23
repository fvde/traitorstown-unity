﻿using System.Collections;
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
    }
}

