
using Kafka;
using Kafka_Multi_Producer;
using Kafka_Multi_Producer.Broker;
using System.Text;

StrartingProducers(40,10000);


void StrartingProducers(short producerAmount, ulong producingAmount)
{
    string topic = "house"; // The topic the data is stored under in Kafka.
    string schemaUrl = "172.16.250.12:8081"; // Ip and port of the schema registry server.
    string bootstrapServer = "172.16.250.13:9092"; // Ip and port of the first kafka broker server.
    InfoPublisher _infoPublisher = Publisher.InfoPublisher;

    House[] houses = {
            new House{Location = "A", ElectricityUsage = 14, HeatingUsage = 1.1, WaterUsage = 0.1},
            new House{Location = "B", ElectricityUsage = 24, HeatingUsage = 1.2, WaterUsage = 1.1},
            new House{Location = "C", ElectricityUsage = 34, HeatingUsage = 1.3, WaterUsage = 0.2},
            new House{Location = "D", ElectricityUsage = 44, HeatingUsage = 1.4, WaterUsage = 3.1},
            new House{Location = "E", ElectricityUsage = 54, HeatingUsage = 1.5, WaterUsage = 0.4},
        };
    
    Random random = new();
    var timeStart = DateTime.Now;
    /* We create a task, that is automatically started, this ensures that the main thread can do other work.
     * After creating the task, the main thread will run the do while until the task 't' has completed.
     */
    Task t = Task.Factory.StartNew( () => Parallel.For(0, producerAmount, index =>
        {
            var house = houses[random.Next(houses.Length)].UniqueHouse();
            house.Location += index.ToString();
            var producer = new Producer(house, schemaUrl, bootstrapServer, topic, _infoPublisher);
            var data = producer.Produce(producingAmount);
            while (!Data.AddValue(data)) ;
        })
    );

    do
    {
        var informations = _infoPublisher.GetInfos(); // Here we are using a publisher to call all producers current produced messages.
        Console.Clear();
        if (informations != null)
        {
            StringBuilder builder = new();
            foreach (var (location, messageProduced) in informations)
            {
                builder.AppendLine($"{location}: {messageProduced}");
            }
            Console.WriteLine(builder.ToString());
            Thread.Sleep(1000);
        }
    } while (!t.IsCompleted);

    var timePassed = DateTime.Now - timeStart;
    Console.WriteLine(timePassed.ToString());
    Console.WriteLine($"Total Produced: {Data.MessageProduced}");
}

