using System.Collections.ObjectModel;
using System.Windows.Input;

namespace KooliProjekt.WpfApplication;
public class MainWindowViewModel : NotifyPropertyChangedBase
{
    private static readonly HttpClient httpClient = new()
    {
        BaseAddress = new Uri("http://localhost:5086/"),
        Timeout = TimeSpan.FromSeconds(15)
    };
    private readonly IApiClient apiClient;
    private readonly IDialogProvider dialogs;
    private readonly CancellationTokenSource lifetime = new();
    private Olu selectedItem;
    private bool isBusy;
    public MainWindowViewModel() : this(new ApiClient(httpClient), new DialogProvider()) { }
    public MainWindowViewModel(IApiClient apiClient, IDialogProvider dialogs)
    {
        this.apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        this.dialogs = dialogs ?? throw new ArgumentNullException(nameof(dialogs));
        AddNewCommand = new RelayCommand<Olu>(_ => { if (IsAvailable) SelectedItem = new Olu(); }, _ => IsAvailable);
        SaveCommand = new RelayCommand<Olu>(async _ => await SaveAsync(), _ => IsAvailable && SelectedItem != null);
        DeleteCommand = new RelayCommand<Olu>(async _ => await DeleteAsync(), _ => IsAvailable && SelectedItem?.Id > 0);
    }
    public ICommand AddNewCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand DeleteCommand { get; }
    public ObservableCollection<Olu> Data { get; } = new();
    public bool IsAvailable => !isBusy && !lifetime.IsCancellationRequested;
    public Olu SelectedItem
    {
        get => selectedItem;
        set
        {
            if (selectedItem == value) return;
            selectedItem = value;
            NotifyPropertyChanged();
            CommandManager.InvalidateRequerySuggested();
        }
    }
    public void CancelPendingOperations() => lifetime.Cancel();
    private async Task RunAsync(Func<Task> action)
    {
        if (!IsAvailable) return;
        isBusy = true;
        NotifyPropertyChanged(nameof(IsAvailable));
        CommandManager.InvalidateRequerySuggested();
        try { await action(); }
        catch (OperationCanceledException) when (lifetime.IsCancellationRequested) { }
        catch (Exception error)
        {
            if (!lifetime.IsCancellationRequested) dialogs.ShowError(error.Message);
        }
        finally
        {
            isBusy = false;
            NotifyPropertyChanged(nameof(IsAvailable));
            CommandManager.InvalidateRequerySuggested();
        }
    }
    public Task InitializeAsync() => RunAsync(async () =>
    {
        var result = await LoadDataAsync(lifetime.Token);
        if (result.HasErrors) ShowError("Andmete laadimine ebaõnnestus.", result);
    });
    public Task SaveAsync() => RunAsync(async () =>
    {
        var item = SelectedItem;
        if (item == null) return;
        var result = await apiClient.Save(item, lifetime.Token);
        lifetime.Token.ThrowIfCancellationRequested();
        if (result.HasErrors) { ShowError("Salvestamine ebaõnnestus.", result); return; }
        // Keep the server ID even if reloading fails, so retrying cannot create a duplicate.
        if (result.Value != null) item.Id = result.Value.Id;
        var id = item.Id;
        var reload = await LoadDataAsync(lifetime.Token);
        if (reload.HasErrors) { ShowError("Salvestatud, kuid nimekirja laadimine ebaõnnestus.", reload); return; }
        SelectedItem = Data.FirstOrDefault(x => x.Id == id);
    });
    public Task DeleteAsync() => RunAsync(async () =>
    {
        var item = SelectedItem;
        if (item == null || item.Id <= 0 || !dialogs.Confirm($"Kas kustutada õlu „{item.Nimi}”?")) return;
        var result = await apiClient.Delete(item.Id, lifetime.Token);
        lifetime.Token.ThrowIfCancellationRequested();
        if (result.HasErrors) { ShowError("Kustutamine ebaõnnestus.", result); return; }
        Data.Remove(item);
        SelectedItem = null;
        var reload = await LoadDataAsync(lifetime.Token);
        if (reload.HasErrors) ShowError("Kustutatud, kuid nimekirja laadimine ebaõnnestus.", reload);
    });
    public void ShowError(string message, OperationResult result)
    {
        var lines = new List<string> { message };
        if (result.Errors != null) lines.AddRange(result.Errors);
        if (result.PropertyErrors != null) lines.AddRange(result.PropertyErrors.Select(x => x.Key + ": " + x.Value));
        dialogs.ShowError(string.Join(Environment.NewLine, lines));
    }
    public async Task<OperationResult> LoadDataAsync(CancellationToken token = default)
    {
        var items = new List<Olu>();
        var page = 1;
        PagedResult<Olu> data;
        do
        {
            token.ThrowIfCancellationRequested();
            var result = await apiClient.List(page, 100, token);
            token.ThrowIfCancellationRequested();
            if (result.HasErrors) return result;
            data = result.Value;
            items.AddRange(data.Results);
            page++;
        } while (page <= data.PageCount);
        SelectedItem = null;
        Data.Clear();
        foreach (var item in items) Data.Add(item);
        return new OperationResult();
    }
}
