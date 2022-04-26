# Kafka-school-experiment

The Producer and Consumer projects require the docker-compose file to be used. Both of the docker-compose files should work, but only the one in kafka-project-docker has been tested.

--Notes--
The schema requires a schema registry to work, is present in both docker-compose files.
It seems like, with how the programs are right now, that Producer needs to be run before Comsumer the first time. 
  The reason for this is that it adds the schema to the registry. 
  The schemas in both ends need to be identical. Comsumer seems to check up against the schema in the schema registry (need to figure out how it finds a specific schema).
    The same goes for the classes, fields/properties need to share names. 
    The class that goes with the schema, needs a public static Schema _SCHEMA with the schema information (can use Schema.Parse(string)).
    
The comsumer project will retrive all data in the given topic each time it is run, even data is has read before.

Need to run:
  docker compose exec broker kafka-topic --create --topic house --bootstrap-server localhost:9092 --replication-factor 1 --partitions 1
The first time the containers are created to create the needed topic.
