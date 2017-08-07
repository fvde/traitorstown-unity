using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traitorstown.src.model;

namespace Traitorstown.src.http.representation
{
    [Serializable]
    public class EffectRepresentation
    {
        public string name;
        public int remainingTurns;

        public EffectRepresentation(string name, int remainingTurns)
        {
            this.name = name;
            this.remainingTurns = remainingTurns;
        }

        public Effect ToEffect()
        {
            return new Effect(name, remainingTurns);
        }
    }
}
