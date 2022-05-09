using Kafka;

namespace Kafka_Comsumer_Data_Handling.Models.Database;

internal class HouseDb
{
    public Guid HouseDbId { get; set; }
    public string Location { get; set; }
 
    //If the data is over a certian n timeUnit old it should be removed. Handled in the logic location

    public IList<(DateTime, IList<(DateTime, double)>)> WaterSamples { get; set; }
    public IList<(DateTime, IList<(DateTime, double)>)> ElectricitySamples { get; set; }
    public IList<(DateTime, IList<(DateTime, double)>)> HeatingSamples { get; set; } 

    private HouseDb()
    {

    }

    public HouseDb(string location)
    {
        Location = location;
    }

    public void ComsumeData(House house)
    {
        if (house.Location.Equals(Location))
        {
            HandleElectricity(house.Reading, house.ElectricityUsage);
            HandleHeating(house.Reading, house.HeatingUsage);
            HandleWater(house.Reading, house.WaterUsage);
        }
    }

    private void HandleWater(DateTime date, double value)
    {
        var samples = WaterSamples.SingleOrDefault(x => x.Item1.Date == date.Date);
        
        if(samples.Item1 == default )
        {
            samples.Item1 = new DateTime(date.Year, date.Month, date.Day);
        }
        if(samples.Item2 == default)
        {
            samples.Item2 = new List<(DateTime, double)>() { };
        }

        samples.Item2.Add((date, value));
    }

    private void HandleHeating(DateTime date, double value)
    {
        var samples = HeatingSamples.SingleOrDefault(x => x.Item1.Date == date.Date);

        if (samples.Item1 == default)
        {
            samples.Item1 = new DateTime(date.Year, date.Month, date.Day);
        }
        if (samples.Item2 == default)
        {
            samples.Item2 = new List<(DateTime, double)>() { };
        }

        samples.Item2.Add((date, value));
    }

    private void HandleElectricity(DateTime date, double value)
    {
        var samples = ElectricitySamples.SingleOrDefault(x => x.Item1.Date == date.Date);

        if (samples.Item1 == default)
        {
            samples.Item1 = new DateTime(date.Year, date.Month, date.Day);
        }
        if (samples.Item2 == default)
        {
            samples.Item2 = new List<(DateTime, double)>() { };
        }

        samples.Item2.Add((date, value));
    }


}
