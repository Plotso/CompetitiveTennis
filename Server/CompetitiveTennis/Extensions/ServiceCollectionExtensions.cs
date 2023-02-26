namespace CompetitiveTennis.Extensions;

using System.Reflection;
using System.Text;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Services;
using Services.Interfaces;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebService<TDbContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment env,
        bool swaggerEnabled)
        where TDbContext : DbContext
    {
        services
            .AddDatabase<TDbContext>(configuration)
            .AddApplicationSettings(configuration)
            .AddTokenAuthentication(configuration, env)
            .AddHealth(configuration)
            .AddMapster(Assembly.GetCallingAssembly())
            .AddControllers();

        if (swaggerEnabled)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services
                .AddEndpointsApiExplorer()
                .AddSwaggerGen();
        }

        return services;
        
    }

    public static IServiceCollection AddMapster(this IServiceCollection services, Assembly assembly)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(assembly);

        return services
            .AddSingleton(config)
            .AddScoped<IMapper, ServiceMapper>();
    }

    public static IServiceCollection AddDatabase<TDbContext>(
        this IServiceCollection services,
        IConfiguration configuration)
        where TDbContext : DbContext
        => services
            .AddScoped<DbContext, TDbContext>()
            .AddDbContext<TDbContext>(options => options
                .UseNpgsql(
                    configuration.GetDefaultConnectionString(),
                    sqlOptions => sqlOptions
                        .EnableRetryOnFailure(
                            maxRetryCount: 10,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorCodesToAdd: null)));

    public static IServiceCollection AddHealth(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddHealthChecks()
            .AddNpgSql(configuration.GetDefaultConnectionString());

        return services;
    }

    public static IServiceCollection AddApplicationSettings(this IServiceCollection services, IConfiguration configuration)
        => services.Configure<ApplicationSettings>(
            configuration.GetSection(nameof(ApplicationSettings)), 
            config => config.BindNonPublicProperties = true);

    public static IServiceCollection AddTokenAuthentication(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment env,
        JwtBearerEvents events = null)
    {
        var secret = configuration
            .GetSection(nameof(ApplicationSettings))
            .GetValue<string>(nameof(ApplicationSettings.Secret));

        var key = Encoding.ASCII.GetBytes(secret);

        services
            .AddAuthentication(authentication =>
            {
                authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(bearer =>
            {
                bearer.RequireHttpsMetadata = !env.IsDevelopment();
                bearer.SaveToken = true;
                bearer.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false, //ToDo: Adjust those
                    ValidateAudience = false
                };

                if (events != null)
                    bearer.Events = events;
            });

        services.AddHttpContextAccessor();
        services.TryAddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
    
    private static string GetDefaultConnectionString(this IConfiguration configuration)
        => configuration.GetConnectionString("DefaultConnection");
}