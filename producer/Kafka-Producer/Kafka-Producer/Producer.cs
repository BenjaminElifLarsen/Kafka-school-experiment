using Confluent.Kafka;
using System;
using Microsoft.Extensions.Configuration;
using Avro.Generic;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Kafka;

namespace Kafka_Producer;

public class Producer
{
    static void Main(string[] args)
    {
        const string topic = "house";

        House[] houses = {
            new House{Location = "A", ElectricityUsage = 14, HeatingUsage = 1.1, WaterUsage = 0.1},
            new House{Location = "B", ElectricityUsage = 24, HeatingUsage = 1.2, WaterUsage = 1.1},
            new House{Location = "C", ElectricityUsage = 34, HeatingUsage = 1.3, WaterUsage = 0.2},
            new House{Location = "D", ElectricityUsage = 44, HeatingUsage = 1.4, WaterUsage = 3.1},
            new House{Location = "E", ElectricityUsage = 54, HeatingUsage = 1.5, WaterUsage = 0.4},
        };

        using (var schemaRegistry = new CachedSchemaRegistryClient(new SchemaRegistryConfig { Url = "localhost:8081" }))
        using (var producer = new ProducerBuilder<Null, House>(new ProducerConfig { BootstrapServers = "localhost:9092" }).SetValueSerializer(new AvroSerializer<House>(schemaRegistry)).Build())
        {
            var numProduced = 0;
            const int numMessage = 10;
            Random random = new();
            for (int i = 0; i < numMessage; i++)
            {
                var house = houses[random.Next(houses.Count())];
                var message = new Message<Null, House> { Value = house };
                producer.ProduceAsync(topic, message 
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

            producer.Flush(TimeSpan.FromSeconds(30));
            Console.WriteLine($"{numProduced} messages were produced to topic {topic}");
        }

    }
}
