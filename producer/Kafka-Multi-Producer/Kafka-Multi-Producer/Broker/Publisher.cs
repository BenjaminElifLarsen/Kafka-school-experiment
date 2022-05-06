namespace Kafka_Multi_Producer.Broker;

/* This class let us acess all of the publishers in the project, 
 * while also ensuring there is only one instance of each publisher.
 */
internal class Publisher
{
    private static InfoPublisher _infoPublisher;

    public static InfoPublisher InfoPublisher => _infoPublisher;

    static Publisher()
    {
        _infoPublisher = new();
    }
}
