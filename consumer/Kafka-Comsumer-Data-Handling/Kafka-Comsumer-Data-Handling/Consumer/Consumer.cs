namespace Kafka_Comsumer_Data_Handling.Consumer;

/*
 * When receving data it should contact the database to retrieve the house on that location if it exist
 * else it should create a new HouseDb entity.
 * In both cases it should then call ComsumeData(House).
 * Finally, it should save to the database.
 * Kind of two logics here, consume kafka data and handle the house object.
 */

internal class Consumer
{
}
