using KooliProjekt.Application.Data;
using KooliProjekt.IntegrationTests.Helpers;
using Xunit;

namespace KooliProjekt.IntegrationTests;

[Collection("SqlIntegration")]
public sealed class MaitsmisedControllerTests : TestBase<Maitsmine>
{
    public MaitsmisedControllerTests(TestDatabaseFixture fixture) : base(fixture) { }
    protected override string Route => "/api/Maitsmised";
    protected override string RequiredField => "Degusteerija";
    protected override Dictionary<string, object> Body(bool updated = false) => new()
    {
        ["Id"] = 0,
        ["PartiiId"] = PartiiId,
        ["Kuupaev"] = new DateTime(2026, 2, updated ? 20 : 19),
        ["Degusteerija"] = updated ? "Other tester" : "Tester",
        ["Hinne"] = updated ? 9 : 7,
        ["Kommentaar"] = updated ? "Updated comment" : "Comment",
    };

    [Fact]
    public Task Save_missing_parent_returns_400() => AssertMissingParent("PartiiId");
}

