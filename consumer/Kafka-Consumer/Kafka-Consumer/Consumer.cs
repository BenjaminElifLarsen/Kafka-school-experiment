using Confluent.Kafka;
using System;
using System.Threading;
using Microsoft.Extensions.Configuration;

namespace Kafka_Consumer;

internal class Consumer
{
    static void Main(string[] args)
    {
        //if(args.Length != 1)
        //{
        //    Console.WriteLine("Please provide the configuration file path as a command line argument");
        //}

        IConfiguration config = new ConfigurationBuilder()
            .AddIniFile("Kafka-Consumer.properties")
            .Build();

        config["group.id"] = "kafka";
        config["auto.offset.reset"] = "earliest";

        const string topic = "water";

        CancellationTokenSource cts = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) => {
            e.Cancel = true; 
            cts.Cancel();
        };

        using (var consumer = new ConsumerBuilder<string, string>(config.AsEnumerable()).Build())
        {
            consumer.Subscribe(topic);
            try
            {
                while (true)
                {
                    var cr = consumer.Consume(cts.Token);
                    Console.WriteLine($"Consumed event from topic {topic} with key {cr.Message.Key,-10} and value {cr.Message.Value}");
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
