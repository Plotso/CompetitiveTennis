using CompetitiveTennis.Extensions;
using CompetitiveTennis.Middlewares;
using CompetitiveTennis.Services;
using CompetitiveTennis.Services.Interfaces;
using CompetitiveTennis.Tournaments.Gateway.Services;
using CompetitiveTennis.Tournaments.Gateway.Services.Tournaments;
using Refit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddTokenAuthentication(builder.Configuration, builder.Environment)
    .AddScoped<ICurrentTokenService, CurrentTokenService>()
    .AddTransient<JwtAuthenticationMiddleware>()
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddControllers();

var serviceEndpoints = builder.Configuration
    .GetSection(nameof(ServiceEndpoints))
    .Get<ServiceEndpoints>(config => config.BindNonPublicProperties = true);


builder.Services.AddRefitClient<IAccountsService>().WithConfiguration(serviceEndpoints.Tournaments);
builder.Services.AddRefitClient<IAvenuesService>().WithConfiguration(serviceEndpoints.Tournaments);
builder.Services.AddRefitClient<ITournamentsService>().WithConfiguration(serviceEndpoints.Tournaments);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Docker"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
    app.UseCors(options => options
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseJwtHeaderAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();