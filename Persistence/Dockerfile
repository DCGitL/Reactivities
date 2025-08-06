# Use the official SQL Server base image
FROM mcr.microsoft.com/mssql/server:2019-latest

# Set environment variables for SQL Server
ENV ACCEPT_EULA=Y
ENV MSSQL_SA_PASSWORD=ActivitiesEvents262!
ENV MSSQL_PID=Developer

# Copy initialization scripts into the container
# These scripts will be executed when the container starts for the first time
# Place your .sql files in a 'scripts' directory next to your Dockerfile
# COPY ./scripts/init.sql /usr/src/app/init.sql

# Expose the SQL Server port
EXPOSE 1433

# You can add commands to run your scripts here, but it's often better
# to let the entrypoint handle it or use an orchestration tool like Docker Compose.
# For simple initial setup, we can use a custom entrypoint or modify the existing one.
# For this example, we'll assume init.sql is a simple script that can be run once SQL Server is up.
# The official image's entrypoint handles running .sql files placed in /usr/src/app/
# if they are part of the image build.
# For more complex scenarios, you might need a custom entrypoint script.

# Example of a simple init.sql (create this file in a 'scripts' directory)
# CREATE DATABASE MyCustomDB;
# GO
# USE MyCustomDB;
# GO
# CREATE LOGIN MyUser WITH PASSWORD = 'MyUserPassword123!', CHECK_POLICY = OFF;
# CREATE USER MyUser FOR LOGIN MyUser;
# ALTER ROLE db_owner ADD MEMBER MyUser;
# GO
