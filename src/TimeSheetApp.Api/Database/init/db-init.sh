#!/bin/bash
echo "waiting 30s for the SQL Server to come up..."
sleep 30s

echo "running set up script..."
/opt/mssql-tools/bin/sqlcmd -S "localhost" -U sa -P "${SA_PASSWORD}" -d master -i /docker-entrypoint-initdb.d/createdb.sql