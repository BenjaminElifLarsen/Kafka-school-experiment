﻿
using Kafka;
using Kafka_Multi_Producer;
using Kafka_Multi_Producer.Broker;

StrartingProducers(3,100);

void StrartingProducers(short producerAmount, ulong producingAmount)
{
    string topic = "house";
    string schemaUrl = "172.16.250.12:8081";
    string bootstrapServer = "172.16.250.13:9092";
    InfoPublisher _infoPublisher = Publisher.InfoPublisher;

    House[] houses = {
            new House{Location = "A", ElectricityUsage = 14, HeatingUsage = 1.1, WaterUsage = 0.1},
            new House{Location = "B", ElectricityUsage = 24, HeatingUsage = 1.2, WaterUsage = 1.1},
            new House{Location = "C", ElectricityUsage = 34, HeatingUsage = 1.3, WaterUsage = 0.2},
            new House{Location = "D", ElectricityUsage = 44, HeatingUsage = 1.4, WaterUsage = 3.1},
            new House{Location = "E", ElectricityUsage = 54, HeatingUsage = 1.5, WaterUsage = 0.4},
        };
    var timeStart = DateTime.Now;
    Random random = new();
    Task t = Task.Factory.StartNew( () => Parallel.For(0, producerAmount, index =>
        {
            var house = houses[random.Next(houses.Length)].UniqueHouse();
            house.Location += index.ToString();
            var producer = new Producer(house, schemaUrl, bootstrapServer, topic, _infoPublisher);
            var result = producer.Produce(producingAmount);
            Console.WriteLine("Index " + index + ": " + result);
        })
    );

    do
    {
        var informations = _infoPublisher.GetInfos();
        Console.Clear();
        foreach(var info in informations)
        {
            Console.WriteLine($"{info.location}: {info.messageProduced}");
            Thread.Sleep(500);
        }
    } while (!t.IsCompleted);

    var timePassed = DateTime.Now - timeStart;
    Console.WriteLine(timePassed.ToString());
}

