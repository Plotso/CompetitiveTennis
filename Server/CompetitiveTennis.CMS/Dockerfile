FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CompetitiveTennis.CMS/CompetitiveTennis.CMS.csproj", "CompetitiveTennis.CMS/"]
COPY ["CompetitiveTennis/CompetitiveTennis.csproj", "CompetitiveTennis/"]
RUN dotnet restore "CompetitiveTennis.CMS/CompetitiveTennis.CMS.csproj"
COPY . .
WORKDIR "/src/CompetitiveTennis.CMS"
RUN dotnet build "CompetitiveTennis.CMS.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CompetitiveTennis.CMS.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CompetitiveTennis.CMS.dll"]
