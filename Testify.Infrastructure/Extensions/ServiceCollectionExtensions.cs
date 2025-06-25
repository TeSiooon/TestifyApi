using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testify.Domain.Entities;
using Testify.Domain.Repositories;
using Testify.Infrastructure.Persistance;
using Testify.Infrastructure.Repositories;

namespace Testify.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("TestifyDb");
        services.AddDbContext<TestifyDbContext>(options =>
            options.UseSqlServer(connectionString).EnableSensitiveDataLogging());

        services.AddIdentityApiEndpoints<User>()
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<TestifyDbContext>();

        services.AddScoped<IQuizRepository, QuizRepository>();
    }
}
