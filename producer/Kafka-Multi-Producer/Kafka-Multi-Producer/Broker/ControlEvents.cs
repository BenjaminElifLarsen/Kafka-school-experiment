namespace Kafka_Multi_Producer.Broker;

/* 
 * This class contains event classes that contains methods and variables used for subscriptions. 
 */
internal class ControlEvents : EventArgs
{
    public class GetProcessEventArgs
    {
        /* 
         * This is private to ensure no outside code can modify it. 
         * It is important to notice that some code calls an event with multiple subscriptions, 
         * only the last returned value is saved. E.g. if Add(...) returned an int, only the last subscribed method called will be saved
         * as the rest are overwritten by those called after.
         * This is the reason for the collection as it allows us to save data from each subscriber. 
         */
        private List<(string location, ulong messageProduced)> producerInfo = new();

        // This get called over in the objects that are subscribed to an event that uses this event class.
        public void Add(string location, ulong messageProduced)
        {
            producerInfo.Add((location, messageProduced));
        }

        /*
         *  Can be used by the code that raised the event to get the data collected.
         */
        public List<(string location, ulong messageProduced)> GetProcessings()
        {
            return producerInfo;
        }
    }




}
