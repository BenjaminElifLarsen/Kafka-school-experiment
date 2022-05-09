using Kafka;
using Kafka_Comsumer_Data_Handling.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafka_Comsumer_Data_Handling.Repositories;

internal class HouseRepository
{
    private readonly BaseRepository<HouseDb> _baseRepository;
    public HouseRepository(BaseRepository<HouseDb> baseRepository)
    {
        _baseRepository = baseRepository;
    }

    public HouseDb GetHouse(House house)
    {
        throw new NotImplementedException();
    }
}
