﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace Traitorstown.src.http.request
{
    [Serializable]
    public class PlayerRequest
    {
        public long id;

        public PlayerRequest(long id)
        {
            this.id = id;
        }
    }
}

