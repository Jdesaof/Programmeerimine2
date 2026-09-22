using System.Net;
using System.Net.Http.Json;
using KooliProjekt.Application.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Xunit;

namespace KooliProjekt.IntegrationTests.Helpers;

public abstract class TestBase<TEntity> : IAsyncLifetime where TEntity : Entity, new()
{
    protected readonly TestDatabaseFixture Fixture;
    protected HttpClient Client => Fixture.Client;
    protected abstract string Route { get; }
    protected abstract string RequiredField { get; }
    protected abstract Dictionary<string, object> Body(bool updated = false);
    protected int OluId;
    protected int PartiiId;
    protected TestBase(TestDatabaseFixture fixture) { Fixture = fixture; }

    public async Task InitializeAsync()
    {
        await Fixture.ResetAsync();
        await WithDb(async db =>
        {
            var olu = new Olu { Nimi = "Parent beer", Tuup = "Lager", Alkoholiprotsent = 5m };
            db.Olud.Add(olu);
            await db.SaveChangesAsync();
            OluId = olu.Id;
            var partii = new Partii { OluId = OluId, Kood = "Parent batch", Kuupaev = new DateTime(2026, 2, 19) };
            db.Partiid.Add(partii);
            await db.SaveChangesAsync();
            PartiiId = partii.Id;
        });
    }
    public Task DisposeAsync() => Task.CompletedTask;

    protected async Task WithDb(Func<ApplicationDbContext, Task> action)
    {
        await using var scope = Fixture.Factory.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await action(db);
    }

    protected async Task<int> Seed()
    {
        var entity = new TEntity();
        foreach (var pair in Body()) typeof(TEntity).GetProperty(pair.Key).SetValue(entity, pair.Value);
        await WithDb(async db => { db.Add(entity); await db.SaveChangesAsync(); });
        return entity.Id;
    }

    private static async Task<JObject> Json(HttpResponseMessage response) =>
        JObject.Parse(await response.Content.ReadAsStringAsync());

    private async Task AssertStored(int id, Dictionary<string, object> body)
    {
        await WithDb(async db =>
        {
            var entity = await db.Set<TEntity>().AsNoTracking().SingleAsync(x => x.Id == id);
            foreach (var pair in body.Where(x => x.Key != "Id"))
                Assert.Equal(pair.Value, typeof(TEntity).GetProperty(pair.Key).GetValue(entity));
        });
    }

    [Fact]
    public async Task List_returns_paged_data()
    {
        var id = await Seed();
        using var response = await Client.GetAsync(Route + "?Page=1&PageSize=100");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var json = await Json(response);
        Assert.Contains(json["value"]["results"], x => (int)x["id"] == id);
        Assert.Equal(1, (int)json["value"]["currentPage"]);
    }

    [Fact]
    public async Task Get_returns_existing_record()
    {
        var id = await Seed();
        using var response = await Client.GetAsync(Route + "/" + id);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(id, (int)(await Json(response))["value"]["id"]);
    }

    [Fact]
    public async Task Get_missing_returns_404()
    {
        using var response = await Client.GetAsync(Route + "/2147483647");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Save_creates_record_and_returns_generated_id()
    {
        var body = Body();
        using var response = await Client.PostAsJsonAsync(Route, body);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var json = await Json(response);
        Assert.NotEqual(true, (bool?)json["hasErrors"]);
        var id = (int)json["value"]["id"];
        Assert.True(id > 0);
        await AssertStored(id, body);
    }

    [Fact]
    public async Task Save_updates_existing_record_without_inserting_another()
    {
        var id = await Seed();
        var body = Body(true);
        body["Id"] = id;
        var before = 0;
        await WithDb(async db => before = await db.Set<TEntity>().CountAsync());
        using var response = await Client.PostAsJsonAsync(Route, body);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(id, (int)(await Json(response))["value"]["id"]);
        await AssertStored(id, body);
        await WithDb(async db => Assert.Equal(before, await db.Set<TEntity>().CountAsync()));
    }

    [Fact]
    public async Task Save_missing_returns_404_without_inserting()
    {
        // Existing project contract: missing Save target returns 404 (teacher sample: 400).
        var body = Body();
        body["Id"] = int.MaxValue;
        var before = 0;
        await WithDb(async db => before = await db.Set<TEntity>().CountAsync());
        using var response = await Client.PostAsJsonAsync(Route, body);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        await WithDb(async db => Assert.Equal(before, await db.Set<TEntity>().CountAsync()));
    }

    [Fact]
    public async Task Save_invalid_returns_400_and_does_not_change_existing_record()
    {
        var id = await Seed();
        var original = Body();
        var body = Body(true);
        body["Id"] = id;
        body[RequiredField] = "";
        using var response = await Client.PostAsJsonAsync(Route, body);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var json = await Json(response);
        Assert.True((bool)json["hasErrors"]);
        Assert.NotNull(((JObject)json["propertyErrors"]).GetValue(RequiredField, StringComparison.OrdinalIgnoreCase));
        await AssertStored(id, original);
    }

    [Fact]
    public async Task Save_invalid_new_record_returns_400_without_inserting()
    {
        var body = Body();
        body[RequiredField] = "";
        var before = 0;
        await WithDb(async db => before = await db.Set<TEntity>().CountAsync());
        using var response = await Client.PostAsJsonAsync(Route, body);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.True((bool)(await Json(response))["hasErrors"]);
        await WithDb(async db => Assert.Equal(before, await db.Set<TEntity>().CountAsync()));
    }

    protected async Task AssertMissingParent(string property)
    {
        var body = Body();
        body[property] = int.MaxValue;
        var before = 0;
        await WithDb(async db => before = await db.Set<TEntity>().CountAsync());
        using var response = await Client.PostAsJsonAsync(Route, body);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var json = await Json(response);
        Assert.True((bool)json["hasErrors"]);
        Assert.NotNull(((JObject)json["propertyErrors"]).GetValue(property, StringComparison.OrdinalIgnoreCase));
        await WithDb(async db => Assert.Equal(before, await db.Set<TEntity>().CountAsync()));
    }

    protected async Task AssertCascade(bool deleteBeer)
    {
        await WithDb(async db =>
        {
            db.Koostisosad.Add(new Koostisosa { PartiiId = PartiiId, Nimetus = "Malt", Uhik = "kg", Kogus = 1m });
            db.Maitsmised.Add(new Maitsmine { PartiiId = PartiiId, Degusteerija = "Tester", Kuupaev = DateTime.Today, Hinne = 7 });
            db.PartiiFotod.Add(new PartiiFoto { PartiiId = PartiiId, FailiTee = "test.jpg" });
            db.PruulimisLogid.Add(new PruulimisLogi { PartiiId = PartiiId, Kasutaja = "Brewer", Kirjeldus = "Test", Kuupaev = DateTime.Today });
            await db.SaveChangesAsync();
        });
        using var response = await Client.DeleteAsync(Route + "/" + (deleteBeer ? OluId : PartiiId));
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        await WithDb(async db =>
        {
            Assert.False(await db.Partiid.AnyAsync(x => x.Id == PartiiId));
            Assert.False(await db.Koostisosad.AnyAsync(x => x.PartiiId == PartiiId));
            Assert.False(await db.Maitsmised.AnyAsync(x => x.PartiiId == PartiiId));
            Assert.False(await db.PartiiFotod.AnyAsync(x => x.PartiiId == PartiiId));
            Assert.False(await db.PruulimisLogid.AnyAsync(x => x.PartiiId == PartiiId));
            Assert.Equal(!deleteBeer, await db.Olud.AnyAsync(x => x.Id == OluId));
        });
    }

    [Fact]
    public async Task Delete_removes_only_target_and_repeated_delete_succeeds()
    {
        var id = await Seed();
        var survivor = await Seed();
        using var response = await Client.DeleteAsync(Route + "/" + id);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotEqual(true, (bool?)(await Json(response))["hasErrors"]);
        await WithDb(async db =>
        {
            Assert.False(await db.Set<TEntity>().AnyAsync(x => x.Id == id));
            Assert.True(await db.Set<TEntity>().AnyAsync(x => x.Id == survivor));
        });
        using var get = await Client.GetAsync(Route + "/" + id);
        Assert.Equal(HttpStatusCode.NotFound, get.StatusCode);
        using var repeated = await Client.DeleteAsync(Route + "/" + id);
        Assert.Equal(HttpStatusCode.OK, repeated.StatusCode);
    }

    [Fact]
    public async Task Delete_missing_is_successful()
    {
        using var response = await Client.DeleteAsync(Route + "/2147483647");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotEqual(true, (bool?)(await Json(response))["hasErrors"]);
    }

    [Fact]
    public async Task Delete_invalid_id_returns_400_without_deleting_data()
    {
        var id = await Seed();
        using var response = await Client.DeleteAsync(Route + "/0");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.True((bool)(await Json(response))["hasErrors"]);
        await WithDb(async db => Assert.True(await db.Set<TEntity>().AnyAsync(x => x.Id == id)));
    }
}
