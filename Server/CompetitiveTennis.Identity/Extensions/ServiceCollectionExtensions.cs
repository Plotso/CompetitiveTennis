namespace CompetitiveTennis.Identity.Extensions;

using Data;
using Data.Models;
using Microsoft.AspNetCore.Identity;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUserStorage(this IServiceCollection services)
    {
        services
            .AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
            })
            .AddEntityFrameworkStores<IdentityDbContext>();

        return services;
    }
}