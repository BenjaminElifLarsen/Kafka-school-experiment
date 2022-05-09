using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Kafka;

namespace Kafka_Comsumer_Data_Handling.Consumer;

/*
 * When receving data it should contact the database to retrieve the house on that location if it exist
 * else it should create a new HouseDb entity.
 * In both cases it should then call ComsumeData(House).
 * Finally, it should save to the database.
 * Kind of two logics here, consume kafka data and handle the house object.
 */

internal class Consumer
{
    private readonly string bsServer = "172.16.250.13:9092";
    private readonly string asrServer = "172.16.250.12:8081";
    private const string _topic = "house";
    private readonly ConsumerConfig _config;
    private readonly CancellationTokenSource _cts = new CancellationTokenSource();

    public Consumer()
    {
        Console.CancelKeyPress += (_, e) => {
            e.Cancel = true;
            _cts.Cancel();
        };
        _config = new()
        {
            GroupId = Guid.NewGuid().ToString(),
            BootstrapServers = bsServer,
            AutoOffsetReset = AutoOffsetReset.Latest
        };
    }

    public void Consume()
    {
        using (var schemaRegistry = new CachedSchemaRegistryClient(new SchemaRegistryConfig { Url = asrServer }))
            using (var consumer = new ConsumerBuilder<Null, House>(_config).SetValueDeserializer(new AvroDeserializer<House>(schemaRegistry).AsSyncOverAsync()).Build())
            {
                consumer.Subscribe(_topic);
                try
                {
                    while (true)
                    {
                        var cr = consumer.Consume(_cts.Token);
                        var house = cr.Value;
                    }
                }
                catch (OperationCanceledException)
                {
                    // Ctrl-C was pressed.
                }
                finally
                {
                    consumer.Close();
                }
            }
    }
}
