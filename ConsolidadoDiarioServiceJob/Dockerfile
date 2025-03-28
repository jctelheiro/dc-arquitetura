FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ConsolidadoDiarioServiceJob/ConsolidadoDiarioServiceJob.csproj", "ConsolidadoDiarioServiceJob/"]
RUN dotnet restore "ConsolidadoDiarioServiceJob/ConsolidadoDiarioServiceJob.csproj"
COPY . .
WORKDIR "/src/ConsolidadoDiarioServiceJob"
RUN dotnet build "ConsolidadoDiarioServiceJob.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ConsolidadoDiarioServiceJob.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConsolidadoDiarioServiceJob.dll"]
