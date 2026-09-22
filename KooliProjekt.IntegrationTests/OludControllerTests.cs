using KooliProjekt.Application.Data;
using KooliProjekt.IntegrationTests.Helpers;
using Xunit;

namespace KooliProjekt.IntegrationTests;

[Collection("SqlIntegration")]
public sealed class OludControllerTests : TestBase<Olu>
{
    public OludControllerTests(TestDatabaseFixture fixture) : base(fixture) { }
    protected override string Route => "/api/Olud";
    protected override string RequiredField => "Nimi";
    protected override Dictionary<string, object> Body(bool updated = false) => new()
    {
        ["Id"] = 0,
        ["Nimi"] = updated ? "Updated beer" : "Test beer",
        ["Kirjeldus"] = updated ? "Updated description" : "Description",
        ["Tuup"] = updated ? "Porter" : "Lager",
        ["Alkoholiprotsent"] = updated ? 6m : 5m,
    };

    [Fact]
    public Task Delete_cascades_to_dependent_records() => AssertCascade(true);
}

