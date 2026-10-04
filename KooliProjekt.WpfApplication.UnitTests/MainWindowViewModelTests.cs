using Moq;
using Xunit;

namespace KooliProjekt.WpfApplication.UnitTests;

public class MainWindowViewModelTests
{
    private readonly Mock<IApiClient> api = new(MockBehavior.Strict);
    private readonly Mock<IDialogProvider> dialogs = new(MockBehavior.Strict);
    private readonly MainWindowViewModel viewModel;

    public MainWindowViewModelTests()
    {
        viewModel = new MainWindowViewModel(api.Object, dialogs.Object);
    }

    private void SetupList(params Olu[] items)
    {
        api.Setup(x => x.List(1, 100, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new OperationResult<PagedResult<Olu>>
            {
                Value = new PagedResult<Olu> { Results = items.ToList(), PageCount = 1 }
            });
    }

    [Fact]
    public void SelectedItem_should_return_correct_item()
    {
        var item = new Olu { Id = 1, Nimi = "Test" };
        viewModel.SelectedItem = item;
        Assert.Same(item, viewModel.SelectedItem);
    }

    [Fact]
    public void SelectedItem_should_call_notify_property_changed()
    {
        var changed = new List<string>();
        viewModel.PropertyChanged += (_, e) => changed.Add(e.PropertyName);
        viewModel.SelectedItem = new Olu { Id = 1, Nimi = "Test" };
        Assert.Contains(nameof(MainWindowViewModel.SelectedItem), changed);
    }

    [Fact]
    public async Task LoadData_should_load_data_from_api_client()
    {
        var first = new Olu { Id = 1, Nimi = "Test 1" };
        var second = new Olu { Id = 2, Nimi = "Test 2" };
        SetupList(first, second);

        var result = await viewModel.LoadDataAsync();

        Assert.False(result.HasErrors);
        Assert.Equal(new[] { first, second }, viewModel.Data.ToArray());
        api.Verify(x => x.List(1, 100, It.IsAny<CancellationToken>()), Times.Once);
        dialogs.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task LoadData_should_show_error_when_api_client_fails()
    {
        api.Setup(x => x.List(1, 100, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new OperationResult<PagedResult<Olu>> { Errors = new() { "API error" } });
        dialogs.Setup(x => x.ShowError(It.IsAny<string>()));

        // InitializeAsync laadib andmed ja kuvab vea dialoogiteenuse kaudu.
        await viewModel.InitializeAsync();

        Assert.Empty(viewModel.Data);
        api.Verify(x => x.List(1, 100, It.IsAny<CancellationToken>()), Times.Once);
        dialogs.Verify(x => x.ShowError(It.Is<string>(s => s.Contains("API error"))), Times.Once);
    }

    [Fact]
    public void AddNew_Command_Should_Set_Empty_SelectedItem()
    {
        var old = new Olu { Id = 5, Nimi = "Vana", Tuup = "Lager", Alkoholiprotsent = 5m, Kirjeldus = "Vana" };
        viewModel.Data.Add(old);
        viewModel.SelectedItem = old;

        viewModel.AddNewCommand.Execute(null);

        var item = Assert.IsType<Olu>(viewModel.SelectedItem);
        Assert.NotSame(old, item);
        Assert.Equal(0, item.Id);
        Assert.True(string.IsNullOrEmpty(item.Nimi));
        Assert.True(string.IsNullOrEmpty(item.Tuup));
        Assert.True(string.IsNullOrEmpty(item.Kirjeldus));
        Assert.Equal(0m, item.Alkoholiprotsent);
        Assert.Same(old, Assert.Single(viewModel.Data));
        api.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task SaveCommand_should_load_data_if_no_errors()
    {
        var draft = new Olu { Nimi = "Uus", Tuup = "Lager", Alkoholiprotsent = 4.5m, Kirjeldus = "Kirjeldus" };
        var saved = new Olu { Id = 18, Nimi = draft.Nimi, Tuup = draft.Tuup, Alkoholiprotsent = draft.Alkoholiprotsent, Kirjeldus = draft.Kirjeldus };
        viewModel.SelectedItem = draft;
        api.Setup(x => x.Save(draft, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new OperationResult<Olu> { Value = saved });
        SetupList(saved);

        // Ootame käsu asünkroonse töö lõppu enne tulemuste kontrollimist.
        await viewModel.SaveAsync();

        api.Verify(x => x.Save(draft, It.IsAny<CancellationToken>()), Times.Once);
        api.Verify(x => x.List(1, 100, It.IsAny<CancellationToken>()), Times.Once);
        Assert.Same(saved, Assert.Single(viewModel.Data));
        Assert.Same(saved, viewModel.SelectedItem);
        Assert.Equal(18, draft.Id);
        dialogs.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task SaveCommand_should_return_when_api_gave_error()
    {
        var draft = new Olu { Nimi = "", Tuup = "Lager", Kirjeldus = "Alles", Alkoholiprotsent = 4m };
        viewModel.SelectedItem = draft;
        api.Setup(x => x.Save(draft, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new OperationResult<Olu> { PropertyErrors = new() { ["Nimi"] = "Nimi on kohustuslik." } });
        dialogs.Setup(x => x.ShowError(It.IsAny<string>()));

        await viewModel.SaveAsync();

        Assert.Same(draft, viewModel.SelectedItem);
        Assert.Equal("Alles", draft.Kirjeldus);
        Assert.Equal(4m, draft.Alkoholiprotsent);
        api.Verify(x => x.Save(draft, It.IsAny<CancellationToken>()), Times.Once);
        api.Verify(x => x.List(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        dialogs.Verify(x => x.ShowError(It.Is<string>(s => s.Contains("Nimi on kohustuslik."))), Times.Once);
    }

    [Fact]
    public void SaveCommand_can_execute_when_selected_item_is_not_null()
    {
        Assert.False(viewModel.SaveCommand.CanExecute(null));
        viewModel.SelectedItem = new Olu();
        Assert.True(viewModel.SaveCommand.CanExecute(viewModel.SelectedItem));
        viewModel.SelectedItem = null;
        Assert.False(viewModel.SaveCommand.CanExecute(null));
    }

    [Fact]
    public async Task DeleteCommand_should_return_when_no_confirmation()
    {
        var item = new Olu { Id = 7, Nimi = "Alles" };
        viewModel.SelectedItem = item;
        viewModel.Data.Add(item);
        dialogs.Setup(x => x.Confirm(It.IsAny<string>())).Returns(false);

        await viewModel.DeleteAsync();

        dialogs.Verify(x => x.Confirm(It.Is<string>(s => s.Contains("Alles"))), Times.Once);
        api.VerifyNoOtherCalls();
        Assert.Same(item, viewModel.SelectedItem);
        Assert.Same(item, Assert.Single(viewModel.Data));
    }

    [Fact]
    public async Task DeleteCommand_should_load_data_if_no_errors()
    {
        var item = new Olu { Id = 7, Nimi = "Kustuta" };
        var remaining = new Olu { Id = 8, Nimi = "Alles" };
        viewModel.SelectedItem = item;
        viewModel.Data.Add(item);
        dialogs.Setup(x => x.Confirm(It.IsAny<string>())).Returns(true);
        api.Setup(x => x.Delete(7, It.IsAny<CancellationToken>())).ReturnsAsync(new OperationResult());
        SetupList(remaining);

        await viewModel.DeleteAsync();

        api.Verify(x => x.Delete(7, It.IsAny<CancellationToken>()), Times.Once);
        api.Verify(x => x.List(1, 100, It.IsAny<CancellationToken>()), Times.Once);
        Assert.Null(viewModel.SelectedItem);
        Assert.Same(remaining, Assert.Single(viewModel.Data));
        dialogs.Verify(x => x.ShowError(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task DeleteCommand_should_return_when_api_gave_error()
    {
        var item = new Olu { Id = 7, Nimi = "Alles" };
        viewModel.SelectedItem = item;
        viewModel.Data.Add(item);
        dialogs.Setup(x => x.Confirm(It.IsAny<string>())).Returns(true);
        dialogs.Setup(x => x.ShowError(It.IsAny<string>()));
        api.Setup(x => x.Delete(7, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new OperationResult { Errors = new() { "Delete error" } });

        await viewModel.DeleteAsync();

        api.Verify(x => x.Delete(7, It.IsAny<CancellationToken>()), Times.Once);
        api.Verify(x => x.List(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        Assert.Same(item, viewModel.SelectedItem);
        Assert.Same(item, Assert.Single(viewModel.Data));
        dialogs.Verify(x => x.ShowError(It.Is<string>(s => s.Contains("Delete error"))), Times.Once);
    }

    [Fact]
    public void DeleteCommand_can_execute_when_selected_item_is_not_null_and_id_is_not_zero()
    {
        Assert.False(viewModel.DeleteCommand.CanExecute(null));
        viewModel.SelectedItem = new Olu();
        Assert.False(viewModel.DeleteCommand.CanExecute(viewModel.SelectedItem));
        viewModel.SelectedItem = new Olu { Id = 7 };
        Assert.True(viewModel.DeleteCommand.CanExecute(viewModel.SelectedItem));
        viewModel.SelectedItem = null;
        Assert.False(viewModel.DeleteCommand.CanExecute(null));
    }
}
