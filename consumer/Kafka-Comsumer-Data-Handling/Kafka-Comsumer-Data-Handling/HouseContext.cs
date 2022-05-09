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
        var converter = new ValueConverter<IList<(DateTime, IList<(DateTime, double)>)>, string>(v => ToDb(v), v => ToEntity(v));

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

    private string ToDb(IList<(DateTime, IList<(DateTime, double)>)> data)
    {
        StringBuilder sb = new();
        foreach(var keyValue in data)
        {
            sb.Append(keyValue.Item1.Ticks);
            sb.Append('-');
            foreach(var dateValue in keyValue.Item2)
            {
                sb.Append(dateValue.Item1.Ticks);
                sb.Append(':');
                sb.Append(dateValue.Item2);
                sb.Append('.');
            }
            sb.Append(';');
        }
        return sb.ToString();
    }

    private IList<(DateTime, IList<(DateTime, double)>)> ToEntity(string data)
    {
        var collection = new List<(DateTime, IList<(DateTime, double)>)>();
        string[] keyValues = data.Split(';',StringSplitOptions.RemoveEmptyEntries);
        foreach(string keyValue in keyValues)
        {
            var split = keyValue.Split('-');
            if (split.Length == 2)
            {
                var keyDate = new DateTime(long.Parse(split[0]));
                var minorCollection = new List<(DateTime, double)>();
                string[] minorKeyValues = split[1].Split('.');
                foreach(string kv in minorKeyValues)
                {
                    var minorSplits = kv.Split(':');
                    var time = new DateTime(long.Parse(minorSplits[0]));
                    var value = double.Parse(minorSplits[1]);
                    minorCollection.Add((time, value));
                }
                collection.Add((keyDate,minorCollection));
            }
        }
        return collection;
    }
}
