namespace Kafka_Multi_Producer.Broker;

internal class InfoPublisher
{
    public delegate void getInfoEventHandler(object sender, ControlEvents.GetProcessEventArgs args);
    public event getInfoEventHandler RaiseGetInfoEvent;

    public List<(string location, ulong messageProduced)> GetInfos()
    {
        return OnGetInfo(new ControlEvents.GetProcessEventArgs());
    }

    protected List<(string location, ulong messageProduced)> OnGetInfo(ControlEvents.GetProcessEventArgs e)
    {
        getInfoEventHandler eventHandler = RaiseGetInfoEvent;
        if(eventHandler != null)
        {
            eventHandler.Invoke(this, e);
            return e.GetProcessings();
        }
        return null;
    }
}
