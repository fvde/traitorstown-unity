using System;
using System.Collections;
using System.Collections.Generic;

namespace Traitorstown.src.http.request
{
    [Serializable]
    public class PlayerReadyRequest
    {
        public bool ready;

        public PlayerReadyRequest(bool ready)
        {
            this.ready = ready;
        }
    }
}

