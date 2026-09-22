using KooliProjekt.Application.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace KooliProjekt.IntegrationTests.Helpers;

[CollectionDefinition("SqlIntegration", DisableParallelization = true)]
public sealed class SqlIntegrationCollection : ICollectionFixture<TestDatabaseFixture> { }

public sealed class TestDatabaseFixture : IAsyncLifetime
{
    public TestApplicationFactory Factory { get; } = new();
    public HttpClient Client { get; private set; }

    public async Task InitializeAsync()
    {
        Client = Factory.CreateClient();
        await using var scope = Factory.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        Factory.AssertOwnedDatabase(db.Database.GetConnectionString());
        await db.Database.EnsureCreatedAsync();
    }

    public async Task ResetAsync()
    {
        await using var scope = Factory.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        Factory.AssertOwnedDatabase(db.Database.GetConnectionString());
        // This fixture owns the GUID-named database. Tests run sequentially.
        await db.PartiiFotod.ExecuteDeleteAsync();
        await db.PruulimisLogid.ExecuteDeleteAsync();
        await db.Maitsmised.ExecuteDeleteAsync();
        await db.Koostisosad.ExecuteDeleteAsync();
        await db.Partiid.ExecuteDeleteAsync();
        await db.Olud.ExecuteDeleteAsync();
    }

    public async Task DisposeAsync()
    {
        try
        {
            if (Client != null)
            {
                await using var scope = Factory.Services.CreateAsyncScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                Factory.AssertOwnedDatabase(db.Database.GetConnectionString());
                await db.Database.EnsureDeletedAsync();
            }
        }
        finally { Client?.Dispose(); await Factory.DisposeAsync(); }
    }
}

