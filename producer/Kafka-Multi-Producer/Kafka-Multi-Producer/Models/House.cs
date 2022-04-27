using Avro;
using Avro.Specific;

namespace Kafka;

internal class House : ISpecificRecord
{
    private Random _rnd = new Random();

    private double _waterUsage;
    private double _electricityUsage;
    private double _heatingUsage;

    public static Schema _SCHEMA = Schema.Parse(File.ReadAllText("models/House.avsc"));
    public string Location { get; set; }
    public double WaterUsage { get => _waterUsage * _rnd.NextDouble() * 2; set => _waterUsage = value; }
    public double ElectricityUsage { get => _electricityUsage * _rnd.NextDouble() * 2; set => _electricityUsage = value; }
    public double HeatingUsage { get => _heatingUsage * _rnd.NextDouble()*2; set => _heatingUsage = value; }

    public Schema Schema => _SCHEMA;

    public object Get(int fieldPos)
    {
        switch (fieldPos)
        {
            case 0: return Location;
            case 1: return WaterUsage;
            case 2: return ElectricityUsage;
            case 3: return HeatingUsage;
            default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Get()");
        }
    }

    public void Put(int fieldPos, object fieldValue)
    {
        switch (fieldPos)
        {
            case 0: Location = (string)fieldValue; break;
            case 1: WaterUsage = (double)fieldValue; break;
            case 2: ElectricityUsage = (double)fieldValue;break;
            case 3: HeatingUsage = (double)fieldValue;break;
            default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Put()");
        }
    }
}
