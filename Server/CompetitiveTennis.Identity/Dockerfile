FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CompetitiveTennis.Identity/CompetitiveTennis.Identity.csproj", "CompetitiveTennis.Identity/"]
COPY ["CompetitiveTennis/CompetitiveTennis.csproj", "CompetitiveTennis/"]
RUN dotnet restore "CompetitiveTennis.Identity/CompetitiveTennis.Identity.csproj"
COPY . .
WORKDIR "/src/CompetitiveTennis.Identity"
RUN dotnet build "CompetitiveTennis.Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CompetitiveTennis.Identity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CompetitiveTennis.Identity.dll"]
