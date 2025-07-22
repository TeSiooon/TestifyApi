using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testify.Application.Abstractions.Repositories;
using Testify.Application.Common;
using Testify.Application.Extensions;
using Testify.Domain.Entities;
using Testify.Infrastructure.Persistance;
using Testify.Infrastructure.Repositories;
using Testify.Infrastructure.Services;
using Testify.IntegrationTests.Helpers;

namespace Testify.IntegrationTests;

public class TestifyFixture : IAsyncLifetime, IDisposable
{
    private readonly ServiceProvider provider;
    private readonly IServiceScope scope;

    public IQuizRepository QuizRepository { get; }
    public IQuestionRepository QuestionRepository { get; }
    public IAnswerRepository AnswerRepository { get; }
    public IMediator Mediator { get; }
    public IUserQuizAttemptRepository UserQuizAttemptRepository { get; }
    public IUserQuizResultRepository UserQuizResultRepository { get; }
    public ICurrentUserService CurrentUserService { get; }
    public UserManager<User> UserManager { get; }

    public TestifyFixture()
    {
        var services = new ServiceCollection();

        // InMemory EF Core
        services.AddDbContext<TestifyDbContext>(opts =>
            opts.UseInMemoryDatabase($"TestifyDb_{Guid.NewGuid()}"));

        services.AddDataProtection();
        services.AddIdentityCore<User>()
                .AddRoles<IdentityRole<Guid>>()                            
                .AddEntityFrameworkStores<TestifyDbContext>()
                .AddDefaultTokenProviders();

        services.AddScoped<IQuizRepository, QuizRepository>();
        services.AddScoped<IQuestionRepository, QuestionRepository>();
        services.AddScoped<IAnswerRepository, AnswerRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserQuizAttemptRepository, UserQuizAttemptRepository>();
        services.AddScoped<IUserQuizResultRepository, UserQuizResultRepository>();

        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddApplication();


        provider = services.BuildServiceProvider();

        scope = provider.CreateScope();

        QuizRepository = scope.ServiceProvider.GetRequiredService<IQuizRepository>();
        QuestionRepository = scope.ServiceProvider.GetRequiredService<IQuestionRepository>();
        AnswerRepository = scope.ServiceProvider.GetRequiredService<IAnswerRepository>();
        Mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        UserQuizAttemptRepository = scope.ServiceProvider.GetRequiredService<IUserQuizAttemptRepository>();
        CurrentUserService = scope.ServiceProvider.GetRequiredService<ICurrentUserService>();
        UserQuizResultRepository = scope.ServiceProvider.GetRequiredService<IUserQuizResultRepository>();
        UserManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    }

    public void Dispose()
    {
        scope.Dispose();
    }

    public async Task DisposeAsync()
    {
        // clear db
        var db = scope.ServiceProvider.GetRequiredService<TestifyDbContext>();
        await db.Database.EnsureDeletedAsync();
    }

    public async Task InitializeAsync()
    {
        // create db
        var db = scope.ServiceProvider.GetRequiredService<TestifyDbContext>();
        await db.Database.EnsureCreatedAsync();
        await CreateTestUserAsync();
    }

    public Task<TResult> ExecuteCommandAsync<TResult>(IRequest<TResult> command)
    {
        var result =  Mediator.Send(command);
        var db = scope.ServiceProvider.GetRequiredService<TestifyDbContext>();
        ClearChangeTracker(db);
        return result;
    }

    public Task<TResponse> ExecuteQueryAsync<TResponse>(IRequest<TResponse> query)
    => Mediator.Send(query);

    private void ClearChangeTracker(TestifyDbContext db)
    {
        db.ChangeTracker.Clear();
    }

    private async Task CreateTestUserAsync()
    {
        var user = new User("testuser", "test@example.com");
        var result = await UserManager.CreateAsync(user, "Test123!");
        if (!result.Succeeded)
            throw new Exception("User creation failed");
    }

    public void SetUserContext(Guid userId, string userName, string email)
    {
        var httpContextAccessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
        httpContextAccessor.HttpContext = HttpContextHelper.PrepareTestHttpContext(userId, userName, email);
    }
    public async Task<User> GetTestUserAsync() => await UserManager.FindByEmailAsync("test@example.com");
}
