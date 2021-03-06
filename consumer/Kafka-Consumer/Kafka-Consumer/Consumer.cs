using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Confluent.Kafka.SyncOverAsync;
using Kafka;

namespace Kafka_Consumer;

internal class Consumer
{
    static void Main(string[] args)
    {

        const string topic = "house";

        CancellationTokenSource cts = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) => {
            e.Cancel = true; 
            cts.Cancel();
        };

        var config = new ConsumerConfig
        {
            GroupId = Guid.NewGuid().ToString(),
            BootstrapServers = "172.16.250.13:9092",
            AutoOffsetReset = AutoOffsetReset.Latest
        };

        using (var schemaRegistry = new CachedSchemaRegistryClient(new SchemaRegistryConfig { Url = "172.16.250.12:8081" }))
        using (var consumer = new ConsumerBuilder<Null, House>(config).SetValueDeserializer(new AvroDeserializer<House>(schemaRegistry).AsSyncOverAsync()).Build())
        {
            consumer.Subscribe(topic);
            try
            {
                while (true)
                {
                    var cr = consumer.Consume(cts.Token);
                    Console.WriteLine(cr.Message.Timestamp.UtcDateTime.ToString("yyyy-MM-dd HH:mm:ss") + $":" + Environment.NewLine +
                        $" Location: {cr.Message.Value.Location}, " + Environment.NewLine +
                        $" Electricity: {cr.Message.Value.ElectricityUsage}, " + Environment.NewLine +
                        $" Heating: {cr.Message.Value.HeatingUsage}, " + Environment.NewLine +
                        $" Water: {cr.Message.Value.WaterUsage}" + Environment.NewLine +
                        $" Reading Time: {cr.Message.Value.Reading}");
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
