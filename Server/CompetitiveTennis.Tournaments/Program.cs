using CompetitiveTennis.Extensions;
using CompetitiveTennis.Services.Interfaces;
using CompetitiveTennis.Tournaments.Data;
using CompetitiveTennis.Tournaments.Services;
using CompetitiveTennis.Tournaments.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddWebService<TournamentsDbContext>(builder.Configuration, builder.Environment, swaggerEnabled: true)
    .AddTransient<IAccountsService, AccountsService>()
    .AddTransient<IAvenuesService, AvenuesService>()
    .AddTransient<ITournamentsService, TournamentsService>()
    .AddTransient<IParticipantsService, ParticipantsService>()
    .AddTransient<ITeamsService, TeamsService>()
    .AddTransient<IMatchesService, MatchesService>()
    .AddTransient<IScoresService, ScoresService>()
    .AddTransient<IDataSeeder, TournamentDataSeeder>();

var app = builder.Build();
app
    .UseWebService(app.Environment, swaggerEnabled: true)
    .SeedData();

app.Run();