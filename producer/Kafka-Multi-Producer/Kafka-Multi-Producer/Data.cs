namespace Kafka_Multi_Producer;
/*
 * Used to store data from all threads in a single value. 
 */
internal static class Data
{
    private static int usingResource = 0;
    public static ulong MessageProduced { get; private set; }
    /*
     * This is needed, since ulong cannot be volatile.
     * This is because a thread can only write a specific amount of bits at a time (depends on the OS)
     * and thus volatile is limited to specific variable types.
     */
    public static bool AddValue(ulong value)
    {
        if(0 == Interlocked.Exchange(ref usingResource, 1)) // This ensure only a single thread can write to the values in the if-statement at a time.
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
