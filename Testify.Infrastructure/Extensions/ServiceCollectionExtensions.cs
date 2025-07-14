using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Testify.Application.Common;
using Testify.Domain.Entities;
using Testify.Domain.Repositories;
using Testify.Infrastructure.Persistance;
using Testify.Infrastructure.Quartz;
using Testify.Infrastructure.Quartz.Jobs;
using Testify.Infrastructure.Repositories;
using Testify.Infrastructure.Services;

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

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IQuizRepository, QuizRepository>();
        services.AddScoped<IQuestionRepository, QuestionRepository>();
        services.AddScoped<IAnswerRepository, AnswerRepository>();
        services.AddScoped<IUserQuizAttemptRepository, UserQuizAttemptRepository>();
        services.AddScoped<IUserQuizResultRepository, UserQuizResultRepository>();

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddQuartzConfigurationAndJobs();
    }
}
