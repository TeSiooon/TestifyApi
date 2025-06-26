using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testify.Application.Extensions;
using Testify.Domain.Repositories;
using Testify.Infrastructure.Persistance;
using Testify.Infrastructure.Repositories;

namespace Testify.IntegrationTests;

public class TestifyFixture : IAsyncLifetime, IDisposable
{
    private readonly IServiceScope scope;
    public IServiceProvider Services => scope.ServiceProvider;

    public TestifyFixture()
    {
        //todo zmienic na WebApplicationFactory aby mogl dzialac na bazie Program.cs i nie duplikowac kodu
        // trzeba bedzie dodac configuracje bazy danych na inmemory
        var services = new ServiceCollection();

        // DbContext InMemory
        services.AddDbContext<TestifyDbContext>(opts =>
            opts.UseInMemoryDatabase($"TestifyDb_{Guid.NewGuid()}"));

        // Rejestracja repozytoriów
        services.AddScoped<IQuizRepository, QuizRepository>();
        services.AddApplication();

        // tu inne zależności, np. AutoMapper, serwisy domenowe

        var provider = services.BuildServiceProvider();
        scope = provider.CreateScope();
    }

    public void Dispose()
    {
        scope.Dispose();
    }

    public async Task DisposeAsync()
    {
        // resetuj baze
        var db = Services.GetRequiredService<TestifyDbContext>();
        await db.Database.EnsureDeletedAsync();
    }

    public async Task InitializeAsync()
    {
        // utworz baze
        var db = Services.GetRequiredService<TestifyDbContext>();
        await db.Database.EnsureCreatedAsync();
    }
}
