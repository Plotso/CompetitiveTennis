namespace CompetitiveTennis.Identity.Data;

using CompetitiveTennis.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Models;

public class IdentitySeeder : IDataSeeder
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public IdentitySeeder(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public void SeedData()
    {
        if (_roleManager.Roles.Any())
            return;

        Task
            .Run(async () =>
            {
                var adminRole = new IdentityRole(Constants.AdministratorRoleName);

                await _roleManager.CreateAsync(adminRole);

                var adminUser = new User
                {
                    UserName = "AdminTester",
                    Email = "adminTester@competitivetennis.com",
                    SecurityStamp = "RandomSecurityStamp",
                    FirstName = "Admin",
                    LastName = "Tester"
                };

                await _userManager.CreateAsync(adminUser, "adminTester@123");

                await _userManager.AddToRoleAsync(adminUser, Constants.AdministratorRoleName);
            })
            .GetAwaiter()
            .GetResult();
    }
}