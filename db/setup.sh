#!/bin/bash
if [ ! -f db_imported ]; then
SERVICE_UP=1
while [[ SERVICE_UP -ne 0 ]]; do
  echo "=> Waiting for confirmation of SQLServer service startup"
  sleep 5
  /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P ${SA_PASSWORD}
  SERVICE_UP=$?
done
/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P ${SA_PASSWORD} -i '/db_setup.sql'
touch db_imported
fi