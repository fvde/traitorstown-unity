﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Traitorstown.src.model
{
    public class Player : TraitorsTownObject
    {
        public long Id { get; set; }

        public Player(long id)
        {
            Id = id;
        }
    }
}