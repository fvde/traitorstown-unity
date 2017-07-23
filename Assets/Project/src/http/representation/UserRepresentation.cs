using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Traitorstown.src.http.representation
{
    [Serializable]
    public class UserRepresentation
    {
        public long id;
        public long playerId;
        public string email;
        public string token;

        public UserRepresentation(long id, long playerId, string email, string token)
        {
            this.id = id;
            this.playerId = playerId;
            this.email = email;
            this.token = token;
        }
    }
}

