using CompetitiveTennis.Extensions;
using CompetitiveTennis.Services.Interfaces;
using CompetitiveTennis.Tournaments.Data;
using CompetitiveTennis.Tournaments.SerializerOptions;
using CompetitiveTennis.Tournaments.Services;
using CompetitiveTennis.Tournaments.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddWebService<TournamentsDbContext>(builder.Configuration, builder.Environment, swaggerEnabled: true,
        enableLegacyTimestampBehaviour: true, dataSourceDelegate: null)
    .AddTransient<IAccountsService, AccountsService>()
    .AddTransient<IAvenuesService, AvenuesService>()
    .AddTransient<ITournamentsService, TournamentsService>()
    .AddTransient<IParticipantsService, ParticipantsService>()
    .AddTransient<ITeamsService, TeamsService>()
    .AddTransient<IMatchesService, MatchesService>()
    .AddTransient<IScoresService, ScoresService>()
    .AddTransient<IDataSeeder, TournamentDataSeeder>()
    .AddSingleton<ISerializerOptions, SerializerOptions>();
/*
.AddNpgsqlDataSource(builder.Configuration.GetConnectionString("DefaultConnection"), sourceBuilder =>
{
sourceBuilder.MapEnum<Surface>(SurfaceEnum);
sourceBuilder.MapEnum<TournamentType>(TournamentTypeEnum);
sourceBuilder.MapEnum<TournamentStage>(TournamentStageEnum);
sourceBuilder.MapEnum<EventStatus>(EventStatusEnum);
sourceBuilder.MapEnum<MatchOutcome>(MatchOutcomeEnum);
}) */

var app = builder.Build();
app
    .UseWebService(app.Environment, swaggerEnabled: true)
    .SeedData();

app.Run();