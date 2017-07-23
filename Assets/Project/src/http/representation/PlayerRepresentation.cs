using System;
using System.Collections;
using System.Collections.Generic;
using Traitorstown.src.model;
using UnityEngine;

namespace Traitorstown.src.http.representation
{
    [Serializable]
    public class PlayerRepresentation
    {
        public long id;
        public bool ready;
        public List<ResourceRepresentation> resources;

        public PlayerRepresentation(long id, bool ready, List<ResourceRepresentation> resources)
        {
            this.id = id;
            this.ready = ready;
            this.resources = resources;
        }

        public Player ToPlayer()
        {
            return new Player((int)id, ready, resources.ConvertAll(resource => resource.ToResource()));
        }
    }
}

