using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Testify.Domain.Entities;
using Testify.Infrastructure.Persistance;

namespace Testify.Infrastructure.Seeders;

public class UserSeeder
{
    private readonly TestifyDbContext dbContext;
    private readonly IConfiguration configuration;
    private readonly UserManager<User> userManager;
    private readonly RoleManager<IdentityRole<Guid>> roleManager;

    public UserSeeder(TestifyDbContext dbContext, IConfiguration configuration,
        UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        this.dbContext = dbContext;
        this.configuration = configuration;
        this.userManager = userManager;
        this.roleManager = roleManager;
    }

    public async Task SeedUsersAsync()
    {
        if((await dbContext.Database.GetPendingMigrationsAsync()).Any())
            await dbContext.Database.MigrateAsync();

        if (!await dbContext.Database.CanConnectAsync())
            return;

        var defaultUsers = configuration.GetSection("DefaultUsers").Get<List<DefaultUser>>();

        if(defaultUsers == null || !defaultUsers.Any())
            return;

        foreach (var defaultUser in defaultUsers)
        {
            if (!await roleManager.RoleExistsAsync(defaultUser.Role))
                await roleManager.CreateAsync(new IdentityRole<Guid>(defaultUser.Role));

            var user = await userManager.FindByNameAsync(defaultUser.UserName);
            if (user == null)
            {
                user = new User(defaultUser.UserName, defaultUser.Email);
                var createResult = await userManager.CreateAsync(user, defaultUser.Password);
                if (!createResult.Succeeded)
                {
                    continue;
                }
                await userManager.AddToRoleAsync(user, defaultUser.Role);
            }
        }
    }
}


public class DefaultUser
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
}
