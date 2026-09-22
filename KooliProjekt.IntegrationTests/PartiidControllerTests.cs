using KooliProjekt.Application.Data;
using KooliProjekt.IntegrationTests.Helpers;
using Xunit;

namespace KooliProjekt.IntegrationTests;

[Collection("SqlIntegration")]
public sealed class PartiidControllerTests : TestBase<Partii>
{
    public PartiidControllerTests(TestDatabaseFixture fixture) : base(fixture) { }
    protected override string Route => "/api/Partiid";
    protected override string RequiredField => "Kood";
    protected override Dictionary<string, object> Body(bool updated = false) => new()
    {
        ["Id"] = 0,
        ["OluId"] = OluId,
        ["Kood"] = updated ? "Updated batch" : "Test batch",
        ["Kuupaev"] = new DateTime(2026, 2, updated ? 20 : 19),
        ["Kirjeldus"] = updated ? "Updated description" : "Description",
        ["Tulemus"] = updated ? "Ready" : "Brewing",
    };

    [Fact]
    public Task Save_missing_parent_returns_400() => AssertMissingParent("OluId");

    [Fact]
    public Task Delete_cascades_to_dependent_records() => AssertCascade(false);
}

