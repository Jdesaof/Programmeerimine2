using System.Collections.ObjectModel;

namespace KooliProjekt.WpfApplication;
public class MainWindowViewModel : NotifyPropertyChangedBase
{
    private static readonly HttpClient httpClient = new()
    {
        BaseAddress = new Uri("http://localhost:5086/"),
        Timeout = TimeSpan.FromSeconds(15)
    };
    private readonly IApiClient apiClient;
    private Olu selectedItem;
    public MainWindowViewModel() : this(new ApiClient(httpClient)) { }
    public MainWindowViewModel(IApiClient apiClient)
    {
        this.apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }
    public ObservableCollection<Olu> Data { get; } = new();
    public Olu SelectedItem
    {
        get => selectedItem;
        set { if (selectedItem == value) return; selectedItem = value; NotifyPropertyChanged(); }
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