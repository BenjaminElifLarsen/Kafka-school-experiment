namespace Kafka_Multi_Producer.Broker;

/* 
 * This class contains event classes that contains methods and variables used for subscriptions. 
 */
internal class ControlEvents : EventArgs
{
    public class GetProcessEventArgs
    {
        /* This is private to ensure no outside code can modify it. 
         * It is important to notice that some code calls an event with multiple subscriptions, 
         * only the last value is saved. E.g. if Add(...) returned an int, only the last subscripted method will be saved
         * as the rest are overwritten.
         * This is the reason for the array as it allows us to save data from each subscriper. 
         */
        private List<(string location, ulong messageProduced)> producerInfo = new List<(string location, ulong messageProduced)>();

        // This get called over in the objects that are subscripted to an event that uses this event class..
        public void Add(string location, ulong messageProduced)
        {
            producerInfo.Add((location, messageProduced));
        }

        public List<(string location, ulong messageProduced)> GetProcessings()
        {
            return producerInfo;
        }
    }




}
