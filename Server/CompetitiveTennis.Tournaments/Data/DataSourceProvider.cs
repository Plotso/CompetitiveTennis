namespace CompetitiveTennis.Tournaments.Data;

using System.Data.Common;
using CompetitiveTennis.Data.Models.Enums;
using Npgsql;

using static DataConstants.CustomDbTypes;

public static class DataSourceProvider
{
    public static Func<string, DbDataSource> GetDataSource => BuildDataSource;

    private static DbDataSource BuildDataSource(string connectionString)
    {
        // Create a data source with the configuration you want:
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataSourceBuilder.MapEnum<Surface>(SurfaceEnum);
        dataSourceBuilder.MapEnum<TournamentType>(TournamentTypeEnum);
        dataSourceBuilder.MapEnum<TournamentStage>(TournamentStageEnum);
        dataSourceBuilder.MapEnum<EventStatus>(EventStatusEnum);
        dataSourceBuilder.MapEnum<MatchOutcome>(MatchOutcomeEnum);

        return dataSourceBuilder.Build();
        /*await using*/
        using var result = DataSourceBuild(dataSourceBuilder).Result;
        return result;
    }

    private static async Task<DbDataSource> DataSourceBuild(NpgsqlDataSourceBuilder builder)
    {
        await using var dataSource = builder.Build();
        return dataSource;
    }
}