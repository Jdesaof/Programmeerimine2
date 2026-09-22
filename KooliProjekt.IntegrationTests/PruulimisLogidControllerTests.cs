using KooliProjekt.Application.Data;
using KooliProjekt.IntegrationTests.Helpers;
using Xunit;

namespace KooliProjekt.IntegrationTests;

[Collection("SqlIntegration")]
public sealed class PruulimisLogidControllerTests : TestBase<PruulimisLogi>
{
    public PruulimisLogidControllerTests(TestDatabaseFixture fixture) : base(fixture) { }
    protected override string Route => "/api/PruulimisLogid";
    protected override string RequiredField => "Kasutaja";
    protected override Dictionary<string, object> Body(bool updated = false) => new()
    {
        ["Id"] = 0,
        ["PartiiId"] = PartiiId,
        ["Kuupaev"] = new DateTime(2026, 2, updated ? 20 : 19),
        ["Kasutaja"] = updated ? "Other brewer" : "Brewer",
        ["Kirjeldus"] = updated ? "Updated log" : "Log",
    };

    [Fact]
    public Task Save_missing_parent_returns_400() => AssertMissingParent("PartiiId");
}

