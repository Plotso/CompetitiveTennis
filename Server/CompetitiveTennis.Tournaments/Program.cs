using CompetitiveTennis.Extensions;
using CompetitiveTennis.Services.Interfaces;
using CompetitiveTennis.Tournaments;
using CompetitiveTennis.Tournaments.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddWebService<TournamentsDbContext>(builder.Configuration, builder.Environment, swaggerEnabled: true,
        enableLegacyTimestampBehaviour: true, dataSourceDelegate: null)
    .AddDataServices()
    .AddBLServices()
    .AddConfigurations(builder.Configuration)
    .AddTransient<IDataSeeder, TournamentDataSeeder>();
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