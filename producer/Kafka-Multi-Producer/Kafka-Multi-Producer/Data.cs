using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafka_Multi_Producer;

internal static class Data
{
    private static int usingResource = 0;
    public static ulong MessageProduced { get; private set; }
    public static bool AddValue(ulong value)
    {
        if(0 == Interlocked.Exchange(ref usingResource, 1))
        {
            Data.MessageProduced += value;
            Interlocked.Exchange(ref usingResource, 0);
            return true;
        }
        else
        {
            return false;
        }
    }
}
