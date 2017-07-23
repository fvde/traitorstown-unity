using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Traitorstown.src.model
{
    public class Cost
    {
        public Resource Resource { get; }
        public int Amount { get; }

        public Cost(Resource resource, int amount)
        {
            this.Resource = resource;
            this.Amount = amount;
        }
    }
}
