using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Traitorstown.src.model
{
    public class Cost
    {
        public Resource resource { get; }
        public int amount { get; }

        public Cost(Resource resource, int amount)
        {
            this.resource = resource;
            this.amount = amount;
        }
    }
}
