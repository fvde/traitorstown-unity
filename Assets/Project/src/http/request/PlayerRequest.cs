using System;
using System.Collections;
using System.Collections.Generic;

namespace Traitorstown.src.http.request
{
    [Serializable]
    public class PlayerRequest
    {
        public int id;

        public PlayerRequest(int id)
        {
            this.id = id;
        }
    }
}

