using Confluent.Kafka;
using System;
using Microsoft.Extensions.Configuration;

namespace Kafka_Producer;

internal class Producer
{
    static void Main(string[] args)
    {
        if(args.Length != 1)
        {
            Console.WriteLine("Please provide the configuration file path as a command line argument");
        }

        IConfiguration config = new ConfigurationBuilder()
            .AddIniFile(args[0])
            .Build();

        const string topic = "water";

        string[] households = new[] { "A1", "A2", "A3", "A4", "A5", "A6a", "A6b", "A7", "B21", "B77", "C21", "D12", "H2" };
        string[] usagePerHourInKilo = new[] { "10", "3", "4.1", "1.1", "0.2", "5"};

        using (var producer = new ProducerBuilder<string, string>(config.AsEnumerable()).Build())
        {
            var numProduced = 0;
            const int numMessage = 10;
            Random random = new Random();
            for(int i = 0; i < numMessage; i++)
            {
                var house = households[random.Next(households.Count())];
                var usage = usagePerHourInKilo[random.Next(usagePerHourInKilo.Count())];

                producer.Produce(topic, new Message<string, string> { Key = house, Value = usage },
                    (deliveryReport) =>
                    {
                        if(deliveryReport.Error != ErrorCode.NoError)
                        {
                            Console.WriteLine($"Failed to deliver message: {deliveryReport.Error}");
                        }
                        else
                        {
                            Console.WriteLine($"Produced event to topic {topic}: key = {house, -10} value = {usage}");
                        }
                    }
                );
            }

            producer.Flush(TimeSpan.FromSeconds(10));
            Console.WriteLine($"{numProduced} messages were produced to topic {topic}");
        }

    }
}
