﻿using System;
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
        public string description;
        public List<CardCostRepresentation> costs;

        public CardRepresentation(long id, string name, string description, List<CardCostRepresentation> costs)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.costs = costs;
        }

        public Card ToCard()
        {
            return new Card((int)id, name, description, costs.ConvertAll(cost => cost.toCost()));
        }
    }
}

