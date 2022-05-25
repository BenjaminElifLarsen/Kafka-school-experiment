using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Kafka;
using Kafka_Multi_Producer.Broker;

namespace Kafka_Multi_Producer;

internal class Producer
{
    protected readonly InfoPublisher _infoPublisher;

    private readonly House _house;
    private readonly string _schemaString;
    private readonly string _topic;
    private readonly string _bootstrapServier;
    private ulong _numProduced = 0;

    public Producer(House house, string schemaUrl, string bootstrapServer, string topic, InfoPublisher infoPublisher)
    {
        _house = house;
        _schemaString = schemaUrl;
        _bootstrapServier = bootstrapServer;
        _topic = topic;
        _infoPublisher = infoPublisher;

        _infoPublisher.RaiseGetInfoEvent += AddInfo; // Here we subscribe the specific method to the specific event, do note that they need to have the same parameters.
    }

    public ulong Produce(ulong amount)
    {
        using (var schemaRegistry = new CachedSchemaRegistryClient(new SchemaRegistryConfig { Url = _schemaString })) {
            using (var producer = new ProducerBuilder<Null, House>(new ProducerConfig { BootstrapServers = _bootstrapServier }).SetValueSerializer(new AvroSerializer<House>(schemaRegistry)).Build())
            {
                for (ulong i = 0; i < amount; i++)
                {
                    var message = new Message<Null, House> { Value = _house };
                    producer.ProduceAsync(_topic, message
                    ).ContinueWith(task =>
                        {
                            _numProduced += (ulong)(task.IsFaulted ? 0 : 1);
                        }
                    ).Wait();
                    _house.UpdateTime(2);
                }
            }
        }
        return _numProduced;
    }

    protected virtual void AddInfo(Object sender, ControlEvents.GetProcessEventArgs e)
    {
        e.Add(_house.Location, _numProduced);
    }
    
    /*
     * Because we are using subscriptions and events to get data from the producers this call here is important.
     * We NEED to unsubscribe any subscribed methods before the garbage collection can be run for the object
     * as the subscripted event holds a reference to the object. 
     */
    public virtual void RemoveSubscriptions()
    {
        _infoPublisher.RaiseGetInfoEvent -= AddInfo;
    }
}
