using Kafka;

namespace Kafka_Comsumer_Data_Handling.Models.Database;

internal class HouseDb
{
    public Guid HouseDbId { get; set; }
    public string Location { get; set; }

    //public DateTime LastDay { get; set; } //maybe replace with another way. Need to handle in cases there are no order in the data.
    //So key-value where key is date and value is tuple of time and usage. 

    public IList<(DateTime, IList<(DateTime, double)>)> WaterSamples { get; set; }
    public IList<(DateTime, IList<(DateTime, double)>)> ElectricitySamples { get; set; }
    public IList<(DateTime, IList<(DateTime, double)>)> HeatingSamples { get; set; } 

    public HouseDb()
    {

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

    //move those to another class
    //private double CalculateAvg(DateTime value, (DateTime, double)[] data) //to be called by all Handles
    //{
    //    var values = data.Where(x => x.Item1.Date == value.Date).Select(x => x.Item2).ToArray();
    //    return values.Sum()/values.Length;
    //}

    //private double CalculateMin(DateTime value, (DateTime, double)[] data)
    //{
    //    var values = data.Where(x => x.Item1.Date == value.Date).Select(x => x.Item2).ToArray();
    //    return values.Min();
    //}

    //private double CalculateMax(DateTime value, (DateTime, double)[] data)
    //{
    //    var values = data.Where(x => x.Item1.Date == value.Date).Select(x => x.Item2).ToArray();
    //    return values.Max();
    //}

}
