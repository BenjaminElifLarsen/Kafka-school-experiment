using Kafka_Comsumer_Data_Handling.Models.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text;

namespace Kafka_Comsumer_Data_Handling;

internal class HouseContext : DbContext
{

    /*
     * Need to have data conversions 
     * : for seperation between data and value
     * ; for seperation between dfferent entities
     * - for seperating key and value
     * . for seperating the key and value of the value mentioned above
     */

    public HouseContext()
    {

    }

    public DbSet<HouseDb> Houses { get; set; }
    public DbSet<OverviewDataDb> Overviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
    string ToDb(IList<(DateTime, IList<(DateTime, double)>)> data)
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

    IList<(DateTime, IList<(DateTime, double)>)> ToEntity(string data)
    {
        var collection = new List<(DateTime, IList<(DateTime, double)>)>();
        string[] keyValues = data.Split(';');
        foreach(string keyValue in keyValues)
        {
            var split = keyValue.Split('-');
            var keyDate = new DateTime(long.Parse(split[0]));
            var minorCollection = new List<(DateTime, double)>();
            if (split.Length == 2)
            {
                string[] minorKeyValues = split[1].Split('.');
                foreach(string kv in minorKeyValues)
                {
                    var minorSplits = kv.Split(':');
                    var time = new DateTime(long.Parse(minorSplits[0]));
                    var value = double.Parse(minorSplits[1]);
                    minorCollection.Add((time, value));
                }
            }
            collection.Add((keyDate,minorCollection));
        }
        return collection;
    }
}
