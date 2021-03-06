using Kafka;

namespace Kafka_Comsumer_Data_Handling.Models.Database;

internal class HouseDb
{
    public Guid HouseDbId { get; set; }
    public string Location { get; set; }
 
    //If the data is over a certian n timeUnit old it should be removed. Handled in the logic location

    public Dictionary<long, IList<Sample<double>>> WaterSamples { get; set; }
    public Dictionary<long, IList<Sample<double>>> ElectricitySamples { get; set; }
    public Dictionary<long, IList<Sample<double>>> HeatingSamples { get; set; } 

    private HouseDb()
    {

    }

    public HouseDb(string location)
    {
        Location = location;
        WaterSamples = new Dictionary<long, IList<Sample<double>>>();
        ElectricitySamples = new Dictionary<long, IList<Sample<double>>>();
        HeatingSamples = new Dictionary<long, IList<Sample<double>>>();
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
        var key = new DateTime(date.Year, date.Month, date.Day).Ticks;
        var samples = WaterSamples[key];
        
        if(samples == default )
        {
            WaterSamples.Add(key, new List<Sample<double>>());
        }

        WaterSamples[key].Add(new Sample<double> { Reading = date, Value = value});
    }

    private void HandleHeating(DateTime date, double value)
    {
        var key = new DateTime(date.Year, date.Month, date.Day).Ticks;
        var samples = HeatingSamples[key];

        if (samples == default)
        {
            HeatingSamples.Add(key, new List<Sample<double>>());
        }

        HeatingSamples[key].Add(new Sample<double> { Reading = date, Value = value });
    }

    private void HandleElectricity(DateTime date, double value)
    {
        var key = new DateTime(date.Year, date.Month, date.Day).Ticks;
        var samples = ElectricitySamples[key];

        if (samples == default)
        {
            ElectricitySamples.Add(key, new List<Sample<double>>());
        }

        ElectricitySamples[key].Add(new Sample<double> { Reading = date, Value = value });
    }


}


internal record Sample<T>
{
    public DateTime Reading { get; set; }
    public T Value { get; set; }
    public Sample()
    {

    }

    public Sample(DateTime reading, T value)
    {
        Reading = reading;
        Value = value;
    }
}