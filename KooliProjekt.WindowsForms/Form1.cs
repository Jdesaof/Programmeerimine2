using System.Net.Http.Json;
using System.Text.Json;

namespace KooliProjekt.WindowsForms;

public partial class Form1 : Form
{
    private readonly HttpClient client = new() { Timeout = TimeSpan.FromSeconds(15) };
    private readonly CancellationTokenSource lifetime = new();
    private const string ListUrl = "http://localhost:5086/api/Olud?Page=1&PageSize=10";

    public Form1() { InitializeComponent(); }

    private async void Form1_Load(object sender, EventArgs e) => await LoadOludAsync();
    private async void ReloadButton_Click(object sender, EventArgs e) => await LoadOludAsync();

    private async Task LoadOludAsync()
    {
        reloadButton.Enabled = false;
        statusLabel.Text = "Laadin andmeid...";
        dataGridView1.DataSource = null;
        try
        {
            var response = await client.GetFromJsonAsync<OperationResult<PagedResult<Olu>>>(ListUrl, lifetime.Token);
            if (lifetime.IsCancellationRequested) return;
            if (response == null || response.HasErrors || response.Value == null)
                throw new InvalidOperationException("API tagastas vigase vastuse.");
            dataGridView1.DataSource = response.Value.Results;
            statusLabel.Text = $"Kuvatud {response.Value.Results.Count} / {response.Value.RowCount} kirjet • Leht {response.Value.CurrentPage}";
        }
        catch (OperationCanceledException) when (lifetime.IsCancellationRequested) { }
        catch (Exception error) when (error is HttpRequestException || error is JsonException
            || error is InvalidOperationException || error is OperationCanceledException)
        {
            if (!lifetime.IsCancellationRequested)
                statusLabel.Text = "Andmeid ei saanud laadida. Käivita WebAPI aadressil http://localhost:5086 ja vajuta Värskenda.";
        }
        finally
        {
            if (!lifetime.IsCancellationRequested) reloadButton.Enabled = true;
        }
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        base.OnFormClosing(e);
        if (!e.Cancel) lifetime.Cancel();
    }
}

