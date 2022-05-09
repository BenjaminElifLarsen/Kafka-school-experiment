using Kafka;
using Kafka_Comsumer_Data_Handling.Models.Database;
using Kafka_Comsumer_Data_Handling.Repositories;

namespace Kafka_Comsumer_Data_Handling.Logics;

internal class ConsumerLogic //find a better name
{
    private readonly HouseRepository _houseRepository; //maybe use unit of work
    private readonly OverviewRepository _overviewRepository;
    private readonly short _maximumCalcuationDayAge = 2;
    private readonly short _maximumSaveDayAge = 5;

    public ConsumerLogic(HouseRepository houseRepository, OverviewRepository overviewRepository)
    {
        _houseRepository = houseRepository;
        _overviewRepository = overviewRepository;
    }

    public void Calculations(House consumerHouse)
    {
        var house = _houseRepository.GetHouse(consumerHouse);
        if(house is null)
        {
            house = new HouseDb(consumerHouse.Location);
        }
        house.ComsumeData(consumerHouse);

        /*
         * Add/Update the context. 
         */

    }

    private void EndOfDate()
    {
        /*
         * Loop for each house in the context.
         * Get any overview they got, else create new ones.
         * Calculate avg heat, water, el for each day.
         * Update/add their overviews in/to the context.
         * Any house with measurement datetime keys that is > n days old will have those removed.
         * Update the houses in the context.
         * Save the context.
         */
        var houses = _houseRepository.All();
        foreach (var house in houses)
        { 
            /*
             * Calculate the values for current date all the way to the current date - _maximumCalcuationDayAge.
             * If any dateTime key in the collections are older than the current date - _maximumSaveDayAge, delete them.
             * Maybe do a final calculation of avg/min/max in case of any consumer data arrived out in order over the current date - _maximumCalcuationDayAge.
             */
            var heatAvgDayOv = _overviewRepository.GetOverview(house.HouseDbId, DateTime.Now, new()
            {
                CalculationType = Models.Enums.CalculationTypes.Avg,
                MeasuringType = Models.Enums.MeasuringTypes.Heating,
                TimePeriods = Models.Enums.TimePeriods.Day
            });

            var waterAvgDayOv = _overviewRepository.GetOverview(house.HouseDbId, DateTime.Now, new()
            {
                CalculationType = Models.Enums.CalculationTypes.Avg,
                MeasuringType = Models.Enums.MeasuringTypes.Water,
                TimePeriods = Models.Enums.TimePeriods.Day
            });

            var elAvgDayOv = _overviewRepository.GetOverview(house.HouseDbId, DateTime.Now, new()
            {
                CalculationType = Models.Enums.CalculationTypes.Avg,
                MeasuringType = Models.Enums.MeasuringTypes.El,
                TimePeriods = Models.Enums.TimePeriods.Day
            });

        }
    }

    private void EndOfWeek()
    {
        /*
         * Runs by the end of the week.
         * Calculate which household is the lowest and the maximum of each measurement in the database.
         * Add overviews with the values.
         */
    }

    private void EndOfMonth()
    {
        /*
         * Runs by the end of the month
         * Calculate which houseHold is the lowest and the highest of each measurement in the database.
         * Add overviews with the values.
         */
    }

    private double CalculateAvg(DateTime value, (DateTime, double)[] data)
    {
        var values = data.Where(x => x.Item1.Date == value.Date).Select(x => x.Item2).ToArray();
        return values.Sum() / values.Length;
    }

    private double CalculateMin(DateTime value, (DateTime, double)[] data)
    {
        var values = data.Where(x => x.Item1.Date == value.Date).Select(x => x.Item2).ToArray();
        return values.Min();
    }

    private double CalculateMax(DateTime value, (DateTime, double)[] data)
    {
        var values = data.Where(x => x.Item1.Date == value.Date).Select(x => x.Item2).ToArray();
        return values.Max();
    }

}
