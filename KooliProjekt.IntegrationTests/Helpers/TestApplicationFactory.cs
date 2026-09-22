using KooliProjekt.Application.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace KooliProjekt.IntegrationTests.Helpers;

public sealed class TestApplicationFactory : WebApplicationFactory<FakeStartup>
{
    // Only this generated database can be created/deleted by this factory.
    public string DatabaseName { get; } = "KooliProjekt_IT_" + Guid.NewGuid().ToString("N");
    public string ConnectionString => new SqlConnectionStringBuilder
    {
        DataSource = @"(localdb)\OlleProjekt", InitialCatalog = DatabaseName,
        IntegratedSecurity = true, TrustServerCertificate = true
    }.ConnectionString;

    public void AssertOwnedDatabase(string connection)
    {
        var parsed = new SqlConnectionStringBuilder(connection);
        if (parsed.InitialCatalog != DatabaseName || parsed.DataSource != @"(localdb)\OlleProjekt"
            || !System.Text.RegularExpressions.Regex.IsMatch(DatabaseName, "^KooliProjekt_IT_[0-9a-f]{32}$"))
            throw new InvalidOperationException("Refusing access to a database not owned by this test factory.");
    }

    protected override IHostBuilder CreateHostBuilder() => Host.CreateDefaultBuilder()
        .ConfigureAppConfiguration(config => config.AddInMemoryCollection(
            new Dictionary<string, string> { ["ConnectionStrings:TestConnection"] = ConnectionString }))
        .ConfigureWebHost(web =>
        {
            web.UseEnvironment("IntegrationTests");
            web.UseContentRoot(AppContext.BaseDirectory);
            web.UseStartup<FakeStartup>();
        });

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseContentRoot(AppContext.BaseDirectory);
    }
}

