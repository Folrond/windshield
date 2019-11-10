using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SensorManager.Helpers
{
    public static class InterlockedHelper
    {
        public static T SafeRead<T>(ref T location) where T : class
        {
            return Interlocked.CompareExchange(ref location, location,location);
        }

        public static void SafeWrite<T>(ref T location, T value) where T : class
        {
            Interlocked.Exchange(ref location, value);
        }

    }
}
