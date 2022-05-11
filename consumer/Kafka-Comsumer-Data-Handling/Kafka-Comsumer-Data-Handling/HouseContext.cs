using Kafka_Comsumer_Data_Handling.Models.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text;

namespace Kafka_Comsumer_Data_Handling;

internal class HouseContext : DbContext
{



    public HouseContext()
    {

    }

    public DbSet<HouseDb> Houses { get; set; }
    public DbSet<OverviewDataDb> Overviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {    
        /*
         * The converter does not handle errors in the data string, v, it tries to convert. For this project it is fine, since the project is a pratice for something else.
         */
        var converter = new ValueConverter<Dictionary<long, IList<Sample<double>>>, string>(v => ToDb(v), v => ToEntity(v));

        modelBuilder.Entity<HouseDb>()
            .Property(e => e.ElectricitySamples)
            .HasConversion(converter);

        modelBuilder.Entity<HouseDb>()
            .Property(e => e.HeatingSamples)
            .HasConversion(converter);

        modelBuilder.Entity<HouseDb>()
            .Property(e => e.WaterSamples)
            .HasConversion(converter);
    }

    /*
     * Data conversions:
     * ; for seperation between dfferent dates.
     * - for seperating dateTime key and (dateTime, IList<dateTime,double>) value.
     * : for seperation between dataTime and double ín the value mentioned above.
     * . for seperating the different keyValues in the value mentioned above.
     * DateTime is converted to long.
     */

    private string ToDb(Dictionary<long, IList<Sample<double>>> data) //make generic
    {
        StringBuilder sb = new();
        foreach(var keyValue in data)
        {
            sb.Append(keyValue.Key);
            sb.Append('-');
            foreach(var dateValue in keyValue.Value)
            {
                sb.Append(dateValue.Reading.Ticks);
                sb.Append(':');
                sb.Append(dateValue.Value);
                sb.Append('.');
            }
            sb.Append(';');
        }
        return sb.ToString();
    }

    private Dictionary<long, IList<Sample<double>>> ToEntity(string data) //make generic
    {
        var collection = new Dictionary<long, IList<Sample<double>>>(); //need to be updated
        string[] keyValues = data.Split(';',StringSplitOptions.RemoveEmptyEntries);
        foreach(string keyValue in keyValues)
        {
            var split = keyValue.Split('-');
            if (split.Length == 2)
            {
                var key = long.Parse(split[0]);
                var minorCollection = new List<Sample<double>>();
                string[] minorKeyValues = split[1].Split('.');
                foreach(string kv in minorKeyValues)
                {
                    var minorSplits = kv.Split(':');
                    var time = new DateTime(long.Parse(minorSplits[0]));
                    var value = double.Parse(minorSplits[1]);
                    minorCollection.Add(new Sample<double> { Reading = time, Value = value});
                }
                collection.Add(key,minorCollection);
            }
        }
        return collection;
    }
}
