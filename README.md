# Kafka-school-experiment

The Producer and Consumer projects can use the docker-compose file to be used, if the Kafka is set up via Docker. Both of the docker-compose files should work, but only the one in kafka-project-docker has been tested. They can also connect to a physical Kafka-cluster. Just remember to set the url values to the correct ones. 

--Notes--

The schema requires a schema registry to work, is present in both docker-compose files.
As it is right now, with how the programs are right now, that Producer needs to be run before Comsumer the first time. 
  The reason for this is that it adds the schema to the registry. 
  The schemas in both ends need to be identical. Comsumer seems to check up against the schema in the schema registry (need to figure out how it finds a specific schema).
    The same goes for the classes, fields/properties need to share names. 
    The class, that goes with the schema, needs a public static Schema _SCHEMA with the schema information (can use Schema.Parse(string)).
    

Need to run:

  docker compose exec broker kafka-topic --create --topic house --bootstrap-server localhost:9092 --replication-factor 1 --partitions 1
  
The first time the containers are created to create the needed topic.
