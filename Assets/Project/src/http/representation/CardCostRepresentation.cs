using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traitorstown.src.model;

namespace Traitorstown.src.http.representation
{
    [Serializable]
    public class CardCostRepresentation
    {
        public Resource resource;
        public int amount;

        public Cost toCost()
        {
            return new Cost(resource, amount);
        }
    }
}
