FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CompetitiveTennis.Tournaments.Gateway/CompetitiveTennis.Tournaments.Gateway.csproj", "CompetitiveTennis.Tournaments.Gateway/"]
COPY ["CompetitiveTennis/CompetitiveTennis.csproj", "CompetitiveTennis/"]
RUN dotnet restore "CompetitiveTennis.Tournaments.Gateway/CompetitiveTennis.Tournaments.Gateway.csproj"
COPY . .
WORKDIR "/src/CompetitiveTennis.Tournaments.Gateway"
RUN dotnet build "CompetitiveTennis.Tournaments.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CompetitiveTennis.Tournaments.Gateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CompetitiveTennis.Tournaments.Gateway.dll"]
