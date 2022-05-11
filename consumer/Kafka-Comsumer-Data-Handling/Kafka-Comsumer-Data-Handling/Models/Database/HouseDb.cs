using Kafka;

namespace Kafka_Comsumer_Data_Handling.Models.Database;

internal class HouseDb
{
    public Guid HouseDbId { get; set; }
    public string Location { get; set; }
 
    //If the data is over a certian n timeUnit old it should be removed. Handled in the logic location

    public Dictionary<DateTime, IList<Sample<double>>> WaterSamples { get; set; }
    public Dictionary<DateTime, IList<Sample<double>>> ElectricitySamples { get; set; }
    public Dictionary<DateTime, IList<Sample<double>>> HeatingSamples { get; set; } 

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
        var key = new DateTime(date.Year, date.Month, date.Day);
        var samples = WaterSamples[new DateTime(date.Year, date.Month, date.Day)];
        
        if(samples == default )
        {
            WaterSamples.Add(key, new List<Sample<double>>());
        }

        WaterSamples[new DateTime(date.Year, date.Month, date.Day)].Add(new Sample<double> { Reading = date, Value = value});
    }

    private void HandleHeating(DateTime date, double value)
    {
        var key = new DateTime(date.Year, date.Month, date.Day);
        var samples = HeatingSamples[key];

        if (samples == default)
        {
            HeatingSamples.Add(key, new List<Sample<double>>());
        }

        HeatingSamples[key].Add(new Sample<double> { Reading = date, Value = value });
    }

    private void HandleElectricity(DateTime date, double value)
    {
        var key = new DateTime(date.Year, date.Month, date.Day);
        var samples = ElectricitySamples[key];

        if (samples == default)
        {
            ElectricitySamples.Add(key, new List<Sample<double>>());
        }

        ElectricitySamples[key].Add(new Sample<double> { Reading = date, Value = value });
    }


}


internal class Sample<T>
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