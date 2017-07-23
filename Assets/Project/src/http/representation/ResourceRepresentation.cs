using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traitorstown.src.model;

namespace Traitorstown.src.http.representation
{
    [Serializable]
    public class ResourceRepresentation
    {
        public ResourceType type;
        public int amount;

        public Resource ToResource()
        {
            return new Resource(type, amount);
        }
    }
}
