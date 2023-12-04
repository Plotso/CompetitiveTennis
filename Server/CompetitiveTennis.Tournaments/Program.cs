using CompetitiveTennis.Extensions;
using CompetitiveTennis.Services.Interfaces;
using CompetitiveTennis.Tournaments.Configurations;
using CompetitiveTennis.Tournaments.Data;
using CompetitiveTennis.Tournaments.SerializerOptions;
using CompetitiveTennis.Tournaments.Services;
using CompetitiveTennis.Tournaments.Services.BL;
using CompetitiveTennis.Tournaments.Services.Interfaces;
using CompetitiveTennis.Tournaments.Services.Interfaces.BL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddWebService<TournamentsDbContext>(builder.Configuration, builder.Environment, swaggerEnabled: true,
        enableLegacyTimestampBehaviour: true, dataSourceDelegate: null)
    .AddScoped<IAccountsService, AccountsService>()
    .AddScoped<IAvenuesService, AvenuesService>()
    .AddScoped<ITournamentsService, TournamentsService>()
    .AddScoped<IParticipantsService, ParticipantsService>()
    .AddScoped<ITeamsService, TeamsService>()
    .AddScoped<IMatchesService, MatchesService>()
    .AddScoped<IScoresService, ScoresService>()
    .AddTransient<IDataSeeder, TournamentDataSeeder>()
    .AddSingleton<ISerializerOptions, SerializerOptions>()
    .AddScoped<ITournamentDrawGenerator, TournamentDrawGenerator>()
    .Configure<TournamentCreationConfiguration>(builder.Configuration.GetSection(nameof(TournamentCreationConfiguration)))
    ;
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