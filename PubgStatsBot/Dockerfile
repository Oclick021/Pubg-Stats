FROM mcr.microsoft.com/dotnet/core/runtime:2.2-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["PubgStatsBot/PubgStatsBot.csproj", "PubgStatsBot/"]
COPY ["PubgSDK/PubgSDK.csproj", "PubgSDK/"]
RUN dotnet restore "PubgStatsBot/PubgStatsBot.csproj"
COPY . .
WORKDIR "/src/PubgStatsBot"
RUN dotnet build "PubgStatsBot.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "PubgStatsBot.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PubgStatsBot.dll"]
