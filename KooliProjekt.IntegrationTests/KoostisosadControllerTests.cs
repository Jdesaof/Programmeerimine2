using KooliProjekt.Application.Data;
using KooliProjekt.IntegrationTests.Helpers;
using Xunit;

namespace KooliProjekt.IntegrationTests;

[Collection("SqlIntegration")]
public sealed class KoostisosadControllerTests : TestBase<Koostisosa>
{
    public KoostisosadControllerTests(TestDatabaseFixture fixture) : base(fixture) { }
    protected override string Route => "/api/Koostisosad";
    protected override string RequiredField => "Nimetus";
    protected override Dictionary<string, object> Body(bool updated = false) => new()
    {
        ["Id"] = 0,
        ["PartiiId"] = PartiiId,
        ["Nimetus"] = updated ? "Updated malt" : "Malt",
        ["Uhik"] = updated ? "g" : "kg",
        ["Hind"] = updated ? 3m : 2m,
        ["Kogus"] = updated ? 6m : 4m,
        ["Kirjeldus"] = updated ? "Updated description" : "Description",
    };

    [Fact]
    public Task Save_missing_parent_returns_400() => AssertMissingParent("PartiiId");
}

