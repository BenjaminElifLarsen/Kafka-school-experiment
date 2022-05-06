namespace Kafka_Multi_Producer.Broker;
/* 
 * This is a publisher that are used to trigger events that classes are subscripted to
 * will react to with the method that they have registrated it with.
 */
internal class InfoPublisher
{
    /*
     * This event handler, RaiseGetInfoEvent, is what classes subscripe to.
     * As it can be seen we use the ControlEvent.GetProcessEventArgs class in the delegate,
     * which means that the subscripted methods can access Add(...) and GetProcessings().
     */
    public delegate void getInfoEventHandler(object sender, ControlEvents.GetProcessEventArgs args);
    public event getInfoEventHandler RaiseGetInfoEvent;

    /*
     * This is called by the code that wants to trigger the event.
     
     */
    public List<(string location, ulong messageProduced)> GetInfos()
    {
        return OnGetInfo(new ControlEvents.GetProcessEventArgs());
    }

    protected List<(string location, ulong messageProduced)> OnGetInfo(ControlEvents.GetProcessEventArgs e)
    {
        getInfoEventHandler eventHandler = RaiseGetInfoEvent;
        if(eventHandler != null) // Only run this code if there are any subscripers 
        {
            eventHandler.Invoke(this, e);
            return e.GetProcessings();
        }
        return null;
    }
}
