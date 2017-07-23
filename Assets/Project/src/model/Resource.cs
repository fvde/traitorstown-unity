using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Traitorstown.src.model
{
    public class Resource
    {
        public ResourceType Type { get; }
        public int Amount { get; }

        public Resource(ResourceType type, int amount)
        {
            Type = type;
            Amount = amount;
        }
    }
}
