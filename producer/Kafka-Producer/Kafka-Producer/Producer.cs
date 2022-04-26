using Confluent.Kafka;
using System;
using Microsoft.Extensions.Configuration;
using Avro.Generic;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;

namespace Kafka_Producer;

public class Producer
{
    static void Main(string[] args)
    {
        //if(args.Length != 1)
        //{
        //    Console.WriteLine("Please provide the configuration file path as a command line argument");
        //}

        //IConfiguration config = new ConfigurationBuilder()
        //    .AddIniFile("Kafka-Producer.properties")
        //    .Build();

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
            //var houseSchema = (Avro.RecordSchema)Avro.Schema.Parse(File.ReadAllText("House.avsc"));


            var numProduced = 0;
            const int numMessage = 10;
            Random random = new Random();
            for (int i = 0; i < numMessage; i++)
            {
                var house = houses[random.Next(houses.Count())];
                //var record = new GenericRecord(houseSchema);
                //record.Add(nameof(house.Location), house.Location);
                //record.Add(nameof(house.WaterUsage), house.WaterUsage);
                //record.Add(nameof(house.ElectricityUsage), house.ElectricityUsage);
                //record.Add(nameof(house.HeatingUsage), house.HeatingUsage);
                var message = new Message<Null, House> { Value = house };
                producer.ProduceAsync(topic, message //,
                    //(deliveryReport) =>
                    //{
                    //    if (deliveryReport.Error != ErrorCode.NoError)
                    //    {
                    //        Console.WriteLine($"Failed to deliver message: {deliveryReport.Error}");
                    //    }
                    //    else
                    //    {
                    //        Console.WriteLine($"Produced event to topic {topic}");
                    //        numProduced += 1;
                    //    }
                    //}
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
