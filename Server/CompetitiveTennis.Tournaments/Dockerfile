FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CompetitiveTennis.Tournaments/CompetitiveTennis.Tournaments.csproj", "CompetitiveTennis.Tournaments/"]
COPY ["CompetitiveTennis.Tournaments.Contracts/CompetitiveTennis.Tournaments.Contracts.csproj", "CompetitiveTennis.Tournaments.Contracts/"]
COPY ["CompetitiveTennis/CompetitiveTennis.csproj", "CompetitiveTennis/"]
RUN dotnet restore "CompetitiveTennis.Tournaments/CompetitiveTennis.Tournaments.csproj"
COPY . .
WORKDIR "/src/CompetitiveTennis.Tournaments"
RUN dotnet build "CompetitiveTennis.Tournaments.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CompetitiveTennis.Tournaments.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CompetitiveTennis.Tournaments.dll"]
