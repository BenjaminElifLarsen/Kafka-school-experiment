using Kafka_Comsumer_Data_Handling.Models.Database;
using Kafka_Comsumer_Data_Handling.Models.Enums;

namespace Kafka_Comsumer_Data_Handling.Repositories;

internal class OverviewRepository
{
    private readonly BaseRepository<OverviewDataDb> _baseRepository;
    public OverviewRepository(BaseRepository<OverviewDataDb> baseRepository)
    {
        _baseRepository = baseRepository;
    }

    public OverviewDataDb GetOverview(Guid houseId, DateTime startDate, DataSearch query)
    {
        throw new NotImplementedException();
    }

    public class DataSearch
    {
        public CalculationTypes? CalculationType { get; set; }
        public MeasuringTypes? MeasuringType { get; set; }
        public TimePeriods? TimePeriods { get; set; }
    }
}
