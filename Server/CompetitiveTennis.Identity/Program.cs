using CompetitiveTennis.Extensions;
using CompetitiveTennis.Identity.Data;
using CompetitiveTennis.Identity.Extensions;
using CompetitiveTennis.Identity.Services;
using CompetitiveTennis.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddWebService<IdentityDbContext>(builder.Configuration, builder.Environment, swaggerEnabled: true)
    .AddUserStorage()
    .AddTransient<IDataSeeder, IdentitySeeder>()
    .AddTransient<IIdentityService, IdentityService>()
    .AddTransient<ITokenGenerator, TokenGenerator>();


var app = builder.Build();
app
    .UseWebService(app.Environment, swaggerEnabled: true)
    .SeedData();

app.Run();