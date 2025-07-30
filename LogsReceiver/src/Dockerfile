#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["LogsReceiver.csproj", "."]
RUN dotnet restore "./LogsReceiver.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "LogsReceiver.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LogsReceiver.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN mkdir -p /var/logs/OPA/status
RUN mkdir -p /var/logs/OPA/decision
ENTRYPOINT ["dotnet", "LogsReceiver.dll"]