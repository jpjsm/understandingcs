# POSTGRESQL REST in .NET Core 7.0

## CLI setup

> ***Note***: these instructions were executed on:  
>>Windows 11 (Version 10.0.22000.2836)  
  .Net SDK Version: 7.0.407

- `dotnet new webapi -n postgres_rest -o . --no-https --auth none --use-program-main`
- `dotnet add package Microsoft.EntityFrameworkCore --version 7.0.17`
- `dotnet add package Microsoft.EntityFrameworkCore.Design --version 7.0.17`
- `dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 7.0.11`
- `dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 7.0.11`
- `dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL.Design`

- `dotnet tool install --global dotnet-ef --version 7.0.17`

## EF From database to model

> ***Note***:
>> This step generates the models for all tables in the database and the context object to use the tables in the code.

- `dotnet ef dbcontext scaffold "Server=oppy-db-01.cec.delllabs.net;Database=business_reporter;Port=5432;User Id=oppy;Password=Mars2004-2018;" Npgsql.EntityFrameworkCore.PostgreSQL -o Model`
- This should add a folder `Model` to the project, add object references (aka files with the name of the tables ending in `.cs`, each file maps a table to a class in the code).
- And added the context object too; something like '*BusinessReporterContext.cs*'.


