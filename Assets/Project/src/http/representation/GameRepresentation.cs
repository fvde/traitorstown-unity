using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Traitorstown.src.http.representation
{
    [Serializable]
    public class GameRepresentation
    {
        public long id;

        public GameRepresentation(long id)
        {
            this.id = id;
        }

        public static GameRepresentation fromJSON(String json)
        {
            return JsonUtility.FromJson<GameRepresentation>(json);
        }
    }
}

