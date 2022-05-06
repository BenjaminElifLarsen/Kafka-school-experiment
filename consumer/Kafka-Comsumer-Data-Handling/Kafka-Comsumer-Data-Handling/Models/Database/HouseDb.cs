using Kafka;
using System.Collections;

namespace Kafka_Comsumer_Data_Handling.Models.Database;

internal class HouseDb
{
    public Guid HouseDbId { get; set; }
    public string Location { get; set; }
    public IList WaterUsasgePerDay { get; set; }
    public IList ElectricityUsagePerDay { get; set; }
    public IList HeatingUsagePerDay { get; set; }
    public DateTime LastDay { get; set; }

    public IList<(DateTime, double)> WaterSamples { get; set; }
    public IList<(DateTime, double)> ElectricitySamples { get; set; }
    public IList<(DateTime, double)> HeatingSamples { get; set; }

    public HouseDb()
    {

    }

    public void ComsumeData(House house)
    {
        if (house.Location.Equals(Location))
        {

        }
    }

}
