using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectZero.Database.Extensions.Utils
{
    public class CircuitBreaker
    {
        private DateTime nextTimeToTry;

        public int ConnectionTimerInterval { get; set; }
        public bool IsBroken { get { return DateTime.Now - nextTimeToTry <= TimeSpan.FromMilliseconds(0); } }


        public CircuitBreaker(int durationBroken)
        {
            ConnectionTimerInterval = durationBroken;
            nextTimeToTry = DateTime.MinValue;
        }

        public void Break()
        {
            nextTimeToTry = DateTime.Now + TimeSpan.FromMilliseconds(ConnectionTimerInterval);
        }


    }
}
