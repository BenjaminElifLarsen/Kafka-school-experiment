using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Kafka;

namespace Kafka_Multi_Producer;

internal class Producer
{
    private readonly House[] _houses;
    private readonly string _schemaString;
    private readonly string _topic;
    private readonly string _bootstrapServier;

    public Producer(House[] houses, string schemaUrl, string bootstrapServer, string topic)
    {
        _houses = houses;
        _schemaString = schemaUrl;
        _bootstrapServier = bootstrapServer;
        _topic = topic;
    }

    public void Produce(object amount)
    {
        using (var schemaRegistry = new CachedSchemaRegistryClient(new SchemaRegistryConfig { Url = _schemaString })) {
            using (var producer = new ProducerBuilder<Null, House>(new ProducerConfig { BootstrapServers = _bootstrapServier }).SetValueSerializer(new AvroSerializer<House>(schemaRegistry)).Build())
            {
                var numProduced = 0;
                Random random = new();
                var house = _houses[random.Next(_houses.Length)];
                for (ulong i = 0; i < (ulong)amount; i++)
                {
                    var message = new Message<Null, House> { Value = house };
                    producer.ProduceAsync(_topic, message
                    ).ContinueWith(task =>
                        {
                            numProduced++;
                            Console.WriteLine(
                              task.IsFaulted
                                  ? $"error producing message: {task.Exception.Message}, {task.Exception.InnerException.InnerException.Message}"
                                  : $"produced to: {task.Result.TopicPartitionOffset}");

                        }
                    ).Wait();
                }
            }
        }
    }

}
