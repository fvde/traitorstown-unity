using System;
using System.Collections;
using System.Collections.Generic;

namespace Traitorstown.src.http.request
{
    [Serializable]
    public class CardRequest
    {
        public long id;

        public CardRequest(long cardId)
        {
            this.id = cardId;
        }
    }
}

