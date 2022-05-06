using Avro;
using Avro.Specific;

namespace Kafka;

internal class House : ISpecificRecord // This interface is required by the Avro we are using.
{
    private Random _rnd = new();

    private double _waterUsage;
    private double _electricityUsage;
    private double _heatingUsage;

    private DateTime _lastReading = DateTime.Now;


    public static Schema _SCHEMA = Schema.Parse(File.ReadAllText("models/House.avsc")); // This variable needs to be present for Avro to work.
    public string Location { get; set; }
    public double WaterUsage { get { return _waterUsage * _rnd.NextDouble() * 2; } set => _waterUsage = value; }
    public double ElectricityUsage { get => _electricityUsage * _rnd.NextDouble() * 2; set => _electricityUsage = value; }
    public double HeatingUsage { get => _heatingUsage * _rnd.NextDouble() * 2; set => _heatingUsage = value; }
    public DateTime Reading { get => _lastReading; set => _lastReading = value; }

    public Schema Schema => _SCHEMA;

    public void UpdateTime(double houseIncreasement)
    {
        _lastReading = _lastReading.AddHours(houseIncreasement);
    }

    /*
     * Used by Avro when serialising the data. The key-values need to be same as in Put(...)
     */
    public object Get(int fieldPos)
    {
        switch (fieldPos)
        {
            case 0: return Location;
            case 1: return WaterUsage;
            case 2: return ElectricityUsage;
            case 3: return HeatingUsage;
            case 4: return Reading.Ticks;
            default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Get()");
        }
    }

    /*
     * Used by Avro when deserialising the data. The key-values need to be same as in Get(...)
     */
    public void Put(int fieldPos, object fieldValue)
    {
        switch (fieldPos)
        {
            case 0: Location = (string)fieldValue; break;
            case 1: WaterUsage = (double)fieldValue; break;
            case 2: ElectricityUsage = (double)fieldValue;break;
            case 3: HeatingUsage = (double)fieldValue;break;
            case 4: Reading = (DateTime)fieldValue; break;
            default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Put()");
        }
    }

    /*
     * This is used to ensure we get an unique version of the house so no reference is shared with other producers.
     */
    public House UniqueHouse()
    {
        return new House
        {
            ElectricityUsage = this.ElectricityUsage,
            HeatingUsage = this.HeatingUsage,
            WaterUsage = this.WaterUsage,
            Location = new string(this.Location.Select(c => c).ToArray()), // Ensures the 'new' Location is not referencing the old Location memory storage.
        };
    }
}
