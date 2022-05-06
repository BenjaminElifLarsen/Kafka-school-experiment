namespace Kafka_Multi_Producer.Broker;

internal class Publisher
{
    private static InfoPublisher _infoPublisher;

    public static InfoPublisher InfoPublisher => _infoPublisher;

    static Publisher()
    {
        _infoPublisher = new();
    }
}
