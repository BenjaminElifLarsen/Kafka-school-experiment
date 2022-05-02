#!/bin/bash
if [ $# -eq 3 ]
then
 curl -X PUT -H "Content-Type: application/vnd.schemaregistry.v1+json" --data '{"compatibility": "'"$1"'"}' $2:8081/config/$3-value
else
 echo "Requires the following: compatibility, url, and topic"
fi


