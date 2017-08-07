using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Traitorstown.src.model
{
    public enum Day
    {
        MONDAY = 1,
        TUESDAY = 2,
        WEDNESDAY = 3,
        THURSDAY = 4,
        FRIDAY = 5,
        SATURDAY = 6,
        SUNDAY = 7
    }

    static class DayMethods
    {
        public static bool IsElectionDay(this Day day)
        {
            return day == Day.SUNDAY;
        }
    }
}
