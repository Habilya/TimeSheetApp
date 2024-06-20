#!/bin/bash
/docker-entrypoint-initdb.d/db-init.sh & /opt/mssql/bin/sqlservr