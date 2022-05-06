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
    private ulong numProduced = 0;

    public Producer(House house, string schemaUrl, string bootstrapServer, string topic, InfoPublisher infoPublisher)
    {
        _house = house;
        _schemaString = schemaUrl;
        _bootstrapServier = bootstrapServer;
        _topic = topic;
        _infoPublisher = infoPublisher;

        _infoPublisher.RaiseGetInfoEvent += AddInfo;
    }

    public void Produce(ulong amount)
    {
        using (var schemaRegistry = new CachedSchemaRegistryClient(new SchemaRegistryConfig { Url = _schemaString })) {
            using (var producer = new ProducerBuilder<Null, House>(new ProducerConfig { BootstrapServers = _bootstrapServier }).SetValueSerializer(new AvroSerializer<House>(schemaRegistry)).Build())
            {
                for (ulong i = 0; i < (ulong)amount; i++)
                {
                    var message = new Message<Null, House> { Value = _house };
                    producer.ProduceAsync(_topic, message
                    ).ContinueWith(task =>
                        {
                            numProduced += (ulong)(task.IsFaulted ? 0 : 1);
                            /*Console.WriteLine(
                              task.IsFaulted
                                  ? $"error producing message: {task.Exception.Message}, {task.Exception.InnerException.InnerException.Message}"
                                  : $"produced to: {task.Result.TopicPartitionOffset}");*/
                        }
                    ).Wait();
                    _house.UpdateTime(2);
                }
            }
        }
        return;
    }

    protected virtual void AddInfo(Object sender, ControlEvents.GetProcessEventArgs e)
    {
        e.Add(_house.Location, numProduced);
    }

}
