using KooliProjekt.WindowsForms.Api;
using Xunit;

namespace KooliProjekt.WindowsForms.UnitTests;

public class MainViewPresenterTests
{
    private readonly FakeApi api = new();
    private readonly FakeView view = new();
    private MainViewPresenter CreatePresenter() => new(api, view);
    private static Olu Item(int id = 7) => new() { Id = id, Nimi = "Test", Tuup = "Lager", Kirjeldus = "Description", Alkoholiprotsent = 5.2m };

    [Fact]
    public async Task LoadData_should_call_ShowError_with_faulty_response()
    {
        var error = new OperationResult<PagedResult<Olu>>().AddError("offline");
        api.ListResponse = _ => error;
        var original = new List<Olu> { Item() };
        view.DataSource = original;
        await CreatePresenter().LoadData();
        Assert.Same(error, Assert.Single(view.Errors).Result);
        Assert.Equal("Viga andmete laadimisel", view.Errors[0].Message);
        Assert.Same(original, view.DataSource);
        Assert.Equal(new[] { 1 }, api.Pages);
    }

    [Fact]
    public async Task LoadData_should_set_DataSource_with_valid_response()
    {
        var first = Item(1);
        var second = Item(2);
        api.ListResponse = page => new(new PagedResult<Olu> { PageCount = 2, Results = new() { page == 1 ? first : second } });
        await CreatePresenter().LoadData();
        Assert.Equal(new[] { first, second }, view.DataSource);
        Assert.Equal(new[] { 1, 2 }, api.Pages);
        Assert.Empty(view.Errors);
    }

    [Fact]
    public void SetSelection_should_clear_fields_with_null_selection()
    {
        view.CurrentId = 7;
        view.CurrentNimi = "Existing";
        view.CurrentTuup = "Lager";
        view.CurrentKirjeldus = "Description";
        view.CurrentAlkoholiprotsent = 5.2m;
        CreatePresenter().SetSelection(null);
        Assert.Equal(0, view.CurrentId);
        Assert.Equal("", view.CurrentNimi);
        Assert.Equal("", view.CurrentTuup);
        Assert.Equal("", view.CurrentKirjeldus);
        Assert.Equal(0m, view.CurrentAlkoholiprotsent);
    }

    [Fact]
    public void SetSelection_should_set_fields_with_valid_selection()
    {
        var item = Item();
        CreatePresenter().SetSelection(item);
        AssertFields(item);
    }

    [Fact]
    public async Task Save_should_call_ShowError_with_faulty_response()
    {
        var presenter = CreatePresenter();
        presenter.SetSelection(Item());
        var error = new OperationResult<Olu>().AddPropertyError("Nimi", "Required");
        api.SaveResponse = error;
        await presenter.Save();
        Assert.Same(error, Assert.Single(view.Errors).Result);
        Assert.Equal("Viga salvestamisel", view.Errors[0].Message);
        Assert.NotNull(api.Saved);
        AssertFields(Item());
        Assert.Empty(api.Pages);
    }

    [Fact]
    public async Task Save_should_call_LoadData_with_valid_response()
    {
        var presenter = CreatePresenter();
        presenter.SetSelection(Item(0));
        var saved = Item(42);
        api.SaveResponse = new(saved);
        api.ListResponse = _ => new(new PagedResult<Olu> { PageCount = 1, Results = new() { saved } });
        await presenter.Save();
        Assert.NotNull(api.Saved);
        Assert.Equal(0, api.Saved.Id);
        Assert.Equal("Test", api.Saved.Nimi);
        Assert.Equal("Lager", api.Saved.Tuup);
        Assert.Equal("Description", api.Saved.Kirjeldus);
        Assert.Equal(5.2m, api.Saved.Alkoholiprotsent);
        Assert.Equal(new[] { 1 }, api.Pages);
        Assert.Same(saved, Assert.Single(view.DataSource));
        Assert.Same(saved, view.SelectedItem);
        AssertFields(saved);
        Assert.Empty(view.Errors);
    }

    [Fact]
    public async Task Delete_should_return_when_user_didnot_confirmed()
    {
        var presenter = CreatePresenter();
        presenter.SetSelection(Item());
        view.Confirm = false;
        await presenter.Delete();
        Assert.Equal(1, view.ConfirmCalls);
        Assert.Empty(api.DeletedIds);
        Assert.Empty(api.Pages);
        Assert.Empty(view.Errors);
        AssertFields(Item());
    }

    [Fact]
    public async Task Delete_should_call_ShowError_with_faulty_response()
    {
        var presenter = CreatePresenter();
        presenter.SetSelection(Item());
        view.Confirm = true;
        var error = new OperationResult().AddError("Delete failed");
        api.DeleteResponse = error;
        await presenter.Delete();
        Assert.Equal(1, view.ConfirmCalls);
        Assert.Equal(new[] { 7 }, api.DeletedIds);
        Assert.Same(error, Assert.Single(view.Errors).Result);
        Assert.Equal("Viga kustutamisel", view.Errors[0].Message);
        Assert.Empty(api.Pages);
        AssertFields(Item());
    }

    private void AssertFields(Olu item)
    {
        Assert.Equal(item.Id, view.CurrentId);
        Assert.Equal(item.Nimi, view.CurrentNimi);
        Assert.Equal(item.Tuup, view.CurrentTuup);
        Assert.Equal(item.Kirjeldus, view.CurrentKirjeldus);
        Assert.Equal(item.Alkoholiprotsent, view.CurrentAlkoholiprotsent);
    }

    private sealed class FakeApi : IApiClient
    {
        public Func<int, OperationResult<PagedResult<Olu>>> ListResponse;
        public OperationResult<Olu> SaveResponse;
        public OperationResult DeleteResponse;
        public List<int> Pages { get; } = new();
        public List<int> DeletedIds { get; } = new();
        public Olu Saved;
        public Task<OperationResult<PagedResult<Olu>>> List(int page, int pageSize, CancellationToken token = default)
        {
            Assert.Equal(100, pageSize);
            Pages.Add(page);
            return Task.FromResult(ListResponse(page));
        }
        public Task<OperationResult<Olu>> Save(Olu item, CancellationToken token = default)
        {
            Saved = item;
            return Task.FromResult(SaveResponse);
        }
        public Task<OperationResult> Delete(int id, CancellationToken token = default)
        {
            DeletedIds.Add(id);
            return Task.FromResult(DeleteResponse);
        }
    }

    private sealed class FakeView : IMainView
    {
        public IList<Olu> DataSource { get; set; }
        public Olu SelectedItem { get; set; }
        public int CurrentId { get; set; }
        public string CurrentNimi { get; set; }
        public string CurrentTuup { get; set; }
        public string CurrentKirjeldus { get; set; }
        public decimal CurrentAlkoholiprotsent { get; set; }
        public bool Confirm;
        public int ConfirmCalls;
        public List<(string Message, OperationResult Result)> Errors { get; } = new();
        public void SetPresenter(MainViewPresenter presenter) { }
        public bool ConfirmDelete() { ConfirmCalls++; return Confirm; }
        public void ShowError(string message, OperationResult result) => Errors.Add((message, result));
    }
}
