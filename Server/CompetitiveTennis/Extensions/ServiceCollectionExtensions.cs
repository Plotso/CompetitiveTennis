namespace CompetitiveTennis.Extensions;

using System.Data.Common;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
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
        bool swaggerEnabled,
        bool enableLegacyTimestampBehaviour,
        Func<string, DbDataSource>? dataSourceDelegate = null)
        where TDbContext : DbContext
    {
        services
            .AddDatabase<TDbContext>(configuration, enableLegacyTimestampBehaviour, dataSourceDelegate)
            .AddApplicationSettings(configuration)
            .AddTokenAuthentication(configuration, env)
            .AddHealth(configuration)
            .AddMapster(Assembly.GetCallingAssembly())
            .AddControllers()
            .AddJsonOptions(x =>
            {
                // serialize enums as strings in api responses (e.g. Role)
                x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

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

    public static IServiceCollection AddDatabase<TDbContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        bool enableLegacyTimestampBehaviour,
        Func<string, DbDataSource>? dataSourceDelegate = null)
        where TDbContext : DbContext
        => dataSourceDelegate == null
            ? services.AddDatabase<TDbContext>(configuration, enableLegacyTimestampBehaviour)
            : services.AddDatabaseWithDataSource<TDbContext>(dataSourceDelegate, configuration,
                enableLegacyTimestampBehaviour);

    public static IServiceCollection AddDatabase<TDbContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        bool enableLegacyTimestampBehaviour)
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
                            errorCodesToAdd: null)))
            .EnablePostgreLegacyTimestampBehaviour(enableLegacyTimestampBehaviour);

    public static IServiceCollection AddDatabaseWithDataSource<TDbContext>(
        this IServiceCollection services,
        Func<string, DbDataSource> dataSourceDelegate,
        IConfiguration configuration,
        bool enableLegacyTimestampBehaviour)
        where TDbContext : DbContext
        => services
            .AddScoped<DbContext, TDbContext>()
            .AddDbContext<TDbContext>(options => options
                .UseNpgsql(
                    dataSourceDelegate(configuration.GetDefaultConnectionString()),
                    sqlOptions => sqlOptions
                        .EnableRetryOnFailure(
                            maxRetryCount: 10,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorCodesToAdd: null)))
            .EnablePostgreLegacyTimestampBehaviour(enableLegacyTimestampBehaviour);

    private static IServiceCollection EnablePostgreLegacyTimestampBehaviour(this IServiceCollection services, bool shouldEnable)
    {
        
        
        if (shouldEnable)
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        return services;
    }

    
    private static string GetDefaultConnectionString(this IConfiguration configuration)
        => configuration.GetConnectionString("DefaultConnection");
}