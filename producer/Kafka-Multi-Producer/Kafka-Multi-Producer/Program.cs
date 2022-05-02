﻿
using Kafka;
using Kafka_Multi_Producer;

StrartingProducers(8,2);

void StrartingProducers(byte threadAmount, ulong producingAmount)
{
    string topic = "house";
    string schemaUrl = "172.16.250.12:8081";
    string bootstrapServer = "172.16.250.13:9092";
    Thread[] threads = new Thread[threadAmount];
    House[] houses = {
            new House{Location = "A", ElectricityUsage = 14, HeatingUsage = 1.1, WaterUsage = 0.1},
            new House{Location = "B", ElectricityUsage = 24, HeatingUsage = 1.2, WaterUsage = 1.1},
            new House{Location = "C", ElectricityUsage = 34, HeatingUsage = 1.3, WaterUsage = 0.2},
            new House{Location = "D", ElectricityUsage = 44, HeatingUsage = 1.4, WaterUsage = 3.1},
            new House{Location = "E", ElectricityUsage = 54, HeatingUsage = 1.5, WaterUsage = 0.4},
        };

    for (int i = 0; i < threadAmount; i++)
    {
        var producer = new Producer(houses, schemaUrl, bootstrapServer, topic);
        threads[i] = new Thread(producer.Produce);
        threads[i].Name = i.ToString();
        threads[i].Start(producingAmount);
    }
}

