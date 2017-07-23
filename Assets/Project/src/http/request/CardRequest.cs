using System;
using System.Collections;
using System.Collections.Generic;

namespace Traitorstown.src.http.request
{
    [Serializable]
    public class CardRequest
    {
        public long id;
        public long target;

        public CardRequest(long id, long target)
        {
            this.id = id;
            this.target = target;
        }
    }
}

