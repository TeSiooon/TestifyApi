using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testify.Application.Common;
using Testify.Application.Extensions;
using Testify.Domain.Repositories;
using Testify.Infrastructure.Persistance;
using Testify.Infrastructure.Repositories;

namespace Testify.IntegrationTests;

public class TestifyFixture : IAsyncLifetime, IDisposable
{
    private readonly ServiceProvider provider;
    private readonly IServiceScope scope;

    public IQuizRepository QuizRepository { get; }
    public IQuestionRepository QuestionRepository { get; }
    public IMediator Mediator { get; }


    public TestifyFixture()
    {
        var services = new ServiceCollection();

        // InMemory EF Core
        services.AddDbContext<TestifyDbContext>(opts =>
            opts.UseInMemoryDatabase($"TestifyDb_{Guid.NewGuid()}"));

        services.AddScoped<IQuizRepository, QuizRepository>();
        services.AddScoped<IQuestionRepository, QuestionRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddApplication();

        provider = services.BuildServiceProvider();

        scope = provider.CreateScope();

        QuizRepository = scope.ServiceProvider.GetRequiredService<IQuizRepository>();
        QuestionRepository = scope.ServiceProvider.GetRequiredService<IQuestionRepository>();
        Mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
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
    }

    public Task<TResult> ExecuteCommandAsync<TResult>(IRequest<TResult> command)
    {
        var result =  Mediator.Send(command);
        var db = scope.ServiceProvider.GetRequiredService<TestifyDbContext>();
        ClearChangeTracker(db);
        return result;
    }

    private void ClearChangeTracker(TestifyDbContext db)
    {
        db.ChangeTracker.Clear();
    }
}
