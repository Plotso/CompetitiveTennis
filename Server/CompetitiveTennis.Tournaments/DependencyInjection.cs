namespace CompetitiveTennis.Tournaments;

using Configurations;
using Services;
using Services.BL;
using Services.Data;
using Services.Interfaces.BL;
using Services.Interfaces.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddDataServices(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddScoped<IAccountsService, AccountsService>()
            .AddScoped<IAvenuesService, AvenuesService>()
            .AddScoped<IMatchesService, MatchesService>()
            .AddScoped<IMatchPeriodsService, MatchPeriodsService>()
            .AddScoped<IParticipantsService, ParticipantsService>()
            .AddScoped<IScoresService, ScoresService>()
            .AddScoped<ITeamsService, TeamsService>()
            .AddScoped<ITournamentsService, TournamentsService>();
    
    public static IServiceCollection AddBLServices(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddScoped<IMatchPeriodInfoManager, MatchPeriodInfoManager>()
            .AddScoped<IMatchOutcomeHandler, MatchOutcomeHandler>()
            .AddScoped<ITournamentDrawGenerator, TournamentDrawGenerator>()
            .AddScoped<IMatchesGenerator, MatchesGenerator>()
            .AddScoped<IAccountStatsProvider, AccountStatsProvider>()
            .AddScoped<IRatingCalculator, RatingCalculator>();

    public static IServiceCollection AddConfigurations(this IServiceCollection serviceCollection,
        IConfiguration configuration)
        => serviceCollection
            .Configure<TournamentCreationConfiguration>(configuration.GetSection(nameof(TournamentCreationConfiguration)))
            .Configure<RatingCalculatorConfiguration>(configuration.GetSection(nameof(RatingCalculatorConfiguration)));
}