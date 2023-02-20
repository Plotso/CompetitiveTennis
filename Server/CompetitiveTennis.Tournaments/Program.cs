using CompetitiveTennis.Extensions;
using CompetitiveTennis.Tournaments.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddWebService<TournamentsDbContext>(builder.Configuration, builder.Environment, swaggerEnabled: true);

var app = builder.Build();
app
    .UseWebService(app.Environment, swaggerEnabled: true)
    .SeedData();

app.Run();