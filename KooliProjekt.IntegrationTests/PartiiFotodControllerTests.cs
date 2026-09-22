using KooliProjekt.Application.Data;
using KooliProjekt.IntegrationTests.Helpers;
using Xunit;

namespace KooliProjekt.IntegrationTests;

[Collection("SqlIntegration")]
public sealed class PartiiFotodControllerTests : TestBase<PartiiFoto>
{
    public PartiiFotodControllerTests(TestDatabaseFixture fixture) : base(fixture) { }
    protected override string Route => "/api/PartiiFotod";
    protected override string RequiredField => "FailiTee";
    protected override Dictionary<string, object> Body(bool updated = false) => new()
    {
        ["Id"] = 0,
        ["PartiiId"] = PartiiId,
        ["FailiTee"] = updated ? "photos/updated.jpg" : "photos/test.jpg",
    };

    [Fact]
    public Task Save_missing_parent_returns_400() => AssertMissingParent("PartiiId");
}

