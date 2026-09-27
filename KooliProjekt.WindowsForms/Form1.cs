using KooliProjekt.WindowsForms.Api;
namespace KooliProjekt.WindowsForms;

public partial class Form1 : Form
{
    private readonly IApiClient apiClient;
    private readonly CancellationTokenSource lifetime = new();
    private bool busy;
    private int currentId;
    public Form1() { InitializeComponent(); }
    public Form1(IApiClient apiClient) : this()
    {
        this.apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }
    private async void Form1_Load(object sender, EventArgs e)
    {
        if (apiClient != null) await RunAction(() => LoadData());
    }
    private async void ReloadButton_Click(object sender, EventArgs e) => await RunAction(() => LoadData(currentId));

    private async Task LoadData(int selectId = 0)
    {
        // The API is paginated; include all pages so a newly added record remains accessible.
        var items = new List<Olu>();
        var page = 1;
        PagedResult<Olu> result;
        do
        {
            var response = await apiClient.List(page, 100, lifetime.Token);
            if (lifetime.IsCancellationRequested) return;
            if (response.HasErrors)
            {
                ShowError("Viga andmete laadimisel", response);
                return;
            }
            result = response.Value;
            items.AddRange(result.Results);
            page++;
        } while (page <= result.PageCount);
        if (lifetime.IsCancellationRequested) return;
        dataGridView1.DataSource = items;
        dataGridView1.ClearSelection();
        ClearFields();
        foreach (DataGridViewRow row in dataGridView1.Rows)
        {
            if (row.DataBoundItem is Olu item && item.Id == selectId)
            {
                row.Selected = true;
                dataGridView1.CurrentCell = row.Cells[0];
                SetFields(item);
                break;
            }
        }
        statusLabel.Text = $"Kuvatud {items.Count} kirjet";
    }

    private void SelectionChanged(object sender, EventArgs e)
    {
        if (dataGridView1.SelectedRows.Count > 0 && dataGridView1.SelectedRows[0].DataBoundItem is Olu item)
            SetFields(item);
    }
    private void SetFields(Olu item)
    {
        currentId = item.Id;
        idBox.Text = item.Id.ToString();
        nameBox.Text = item.Nimi;
        typeBox.Text = item.Tuup;
        descriptionBox.Text = item.Kirjeldus;
        alcoholBox.Value = Math.Clamp(item.Alkoholiprotsent, 0m, 100m);
        deleteButton.Enabled = !busy && currentId > 0;
    }
    private void ClearFields()
    {
        currentId = 0;
        idBox.Text = "Uus";
        nameBox.Clear();
        typeBox.Clear();
        descriptionBox.Clear();
        alcoholBox.Value = 0;
        deleteButton.Enabled = false;
    }
    private void AddButton_Click(object sender, EventArgs e)
    {
        dataGridView1.ClearSelection();
        ClearFields();
        nameBox.Focus();
        statusLabel.Text = "Uus kirje — täida väljad ja vajuta Salvesta.";
    }
    private async void SaveButton_Click(object sender, EventArgs e) => await RunAction(async () =>
    {
        var saved = await apiClient.Save(new Olu
        {
            Id = currentId, Nimi = nameBox.Text, Tuup = typeBox.Text,
            Kirjeldus = descriptionBox.Text, Alkoholiprotsent = alcoholBox.Value
        }, lifetime.Token);
        if (lifetime.IsCancellationRequested) return;
        if (saved.HasErrors)
        {
            ShowError("Viga salvestamisel", saved);
            return;
        }
        await LoadData(saved.Value.Id);
    });
    private async void DeleteButton_Click(object sender, EventArgs e)
    {
        var id = currentId;
        if (id <= 0 || MessageBox.Show(this, $"Kustutada õlu \"{nameBox.Text}\"?",
            "Kinnita kustutamine", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        await RunAction(async () =>
        {
            var result = await apiClient.Delete(id, lifetime.Token);
            if (lifetime.IsCancellationRequested) return;
            if (result.HasErrors)
            {
                ShowError("Viga kustutamisel", result);
                return;
            }
            await LoadData();
        });
    }
    // Adapted from the teacher's 20.03 ShowError: display both kinds of API errors.
    private void ShowError(string message, OperationResult result)
    {
        var error = message + "\r\n";
        var apiErrors = "";
        var propertyErrors = "";
        if (result.Errors != null)
            foreach (var apiError in result.Errors) apiErrors += apiError + "\r\n";
        if (result.PropertyErrors != null)
            foreach (var propertyError in result.PropertyErrors)
                propertyErrors += propertyError.Key + ": " + propertyError.Value + "\r\n";
        if (!string.IsNullOrEmpty(apiErrors)) error += "\r\n" + apiErrors;
        if (!string.IsNullOrEmpty(propertyErrors)) error += "\r\n" + propertyErrors;
        statusLabel.Text = message;
        MessageBox.Show(this, error.Trim(), "Viga!", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
    private async Task RunAction(Func<Task> action)
    {
        if (busy) return;
        SetBusy(true);
        statusLabel.Text = "Palun oota...";
        try { await action(); }
        catch (OperationCanceledException) when (lifetime.IsCancellationRequested) { }
        catch (Exception error) when (error is HttpRequestException || error is InvalidOperationException
            || error is Newtonsoft.Json.JsonException || error is OperationCanceledException)
        {
            if (!lifetime.IsCancellationRequested)
            {
                ShowError("Toiming ebaõnnestus", new OperationResult().AddError(error.Message));
            }
        }
        finally { if (!lifetime.IsCancellationRequested) SetBusy(false); }
    }
    private void SetBusy(bool value)
    {
        busy = value;
        dataGridView1.Enabled = !value;
        editor.Enabled = !value;
        reloadButton.Enabled = !value;
        addButton.Enabled = !value;
        saveButton.Enabled = !value;
        deleteButton.Enabled = !value && currentId > 0;
    }
    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        base.OnFormClosing(e);
        if (!e.Cancel) lifetime.Cancel();
    }
}

