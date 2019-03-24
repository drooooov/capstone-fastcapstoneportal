FROM microsoft/mssql-server-linux:latest

ENV ACCEPT_EULA Y

COPY db_setup.sql setup.sh /
CMD /bin/bash /setup.sh & /opt/mssql/bin/sqlservr