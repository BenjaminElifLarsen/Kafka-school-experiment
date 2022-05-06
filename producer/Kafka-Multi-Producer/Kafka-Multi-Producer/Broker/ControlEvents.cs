namespace Kafka_Multi_Producer.Broker;

internal class ControlEvents : EventArgs
{
    public class GetProcessEventArgs
    {
        private List<(string location, ulong messageProduced)> producerInfo = new List<(string location, ulong messageProduced)>();

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
