using Kafka_Comsumer_Data_Handling.Models.Enums;

namespace Kafka_Comsumer_Data_Handling.Models.Database;

internal class OverviewDataDb
{
    public Guid OverviewDataDbId { get; set; }
    public HouseDb HouseDb { get; set; }
    public TimePeriods TimePeriod { get; set; }
    public MeasuringTypes MeasuringType { get; set; }
    public CalculationTypes CalculationType { get; set; }
    public double Value { get; set; }
    public DateTime StartDate { get; set; }

    public OverviewDataDb(HouseDb house, TimePeriods timePeriod, MeasuringTypes measuringType, CalculationTypes calculationType, double value, DateTime startDate)
    {
        HouseDb = house;
        TimePeriod = timePeriod;
        MeasuringType = measuringType;
        CalculationType = calculationType;
        Value = value;
        StartDate = startDate;
    }
}
