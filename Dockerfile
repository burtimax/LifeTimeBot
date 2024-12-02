FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# dotnet publish -c Release -p:UseAppHost=false
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

FROM build AS publish

FROM base AS final
# WORKDIR /app
COPY ["publish/", "/app/"]
ENTRYPOINT ["dotnet", "LifeTimeBot.dll"]