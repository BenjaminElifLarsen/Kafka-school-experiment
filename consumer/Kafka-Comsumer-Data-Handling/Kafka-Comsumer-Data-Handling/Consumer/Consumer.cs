using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Kafka;
using Kafka_Comsumer_Data_Handling.Logics;
using Kafka_Comsumer_Data_Handling.Repositories;

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
    private readonly string _bsServer = "172.16.250.13:9092";
    private readonly string _asrServer = "172.16.250.12:8081";
    private const string _topic = "house";
    private readonly ConsumerConfig _config;
    private readonly CancellationTokenSource _cts = new();
    private readonly HouseRepository _houseRepository; //move this and the other repo into a Unit of Work
    private readonly OverviewRepository _overviewRepository;

    public Consumer(HouseRepository houseRepository, OverviewRepository overviewRepository) : this()
    {
        _houseRepository = houseRepository;
        _overviewRepository = overviewRepository;
    }

    private Consumer()
    {
        Console.CancelKeyPress += (_, e) => {
            e.Cancel = true;
            _cts.Cancel();
        };
        _config = new()
        {
            GroupId = Guid.NewGuid().ToString(),
            BootstrapServers = _bsServer,
            AutoOffsetReset = AutoOffsetReset.Latest
        };
    }

    public void Consume()
    {
        var houseLogic = new ConsumerLogic(_houseRepository, _overviewRepository);
        using (var schemaRegistry = new CachedSchemaRegistryClient(new SchemaRegistryConfig { Url = _asrServer }))
            using (var consumer = new ConsumerBuilder<Null, House>(_config).SetValueDeserializer(new AvroDeserializer<House>(schemaRegistry).AsSyncOverAsync()).Build())
            {
                consumer.Subscribe(_topic);
                try
                {
                    while (true)
                    {
                        var cr = consumer.Consume(_cts.Token);
                        var house = cr.Message.Value;
                        houseLogic.Calculations(house);
                    }
                }
                catch (OperationCanceledException)
                {
                    // Ctrl-C was pressed.
                }
                catch (Exception e)
                {
                    // Log the error.
                }
                finally
                {
                    consumer.Close();
                }
            }
    }
}
