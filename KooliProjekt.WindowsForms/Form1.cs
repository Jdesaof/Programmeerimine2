using System.ComponentModel;

namespace KooliProjekt.WindowsForms;

public partial class Form1 : Form, IMainView
{

    private readonly CancellationTokenSource lifetime = new();
    private bool busy;
    private MainViewPresenter presenter;
    public Form1() { InitializeComponent(); }
    private async void Form1_Load(object sender, EventArgs e)
    {
        if (presenter != null) await RunAction(() => LoadData());
    }
    private async void ReloadButton_Click(object sender, EventArgs e) => await RunAction(() => LoadData(CurrentId));

    private Task LoadData(int selectId = 0) => presenter.LoadData(selectId, lifetime.Token);

    public void SetPresenter(MainViewPresenter value) =>
        presenter = value ?? throw new ArgumentNullException(nameof(value));

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public IList<Olu> DataSource
    {
        get => dataGridView1.DataSource as IList<Olu>;
        set
        {
            dataGridView1.DataSource = value;
            statusLabel.Text = $"Kuvatud {value?.Count ?? 0} kirjet";
        }
    }
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Olu SelectedItem
    {
        get => dataGridView1.SelectedRows.Count > 0
            ? dataGridView1.SelectedRows[0].DataBoundItem as Olu : null;
        set
        {
            dataGridView1.ClearSelection();
            dataGridView1.CurrentCell = null;
            if (value == null) return;
            foreach (DataGridViewRow row in dataGridView1.Rows)
                if (row.DataBoundItem is Olu item && item.Id == value.Id)
                {
                    dataGridView1.CurrentCell = row.Cells[0];
                    row.Selected = true;
                    break;
                }
        }
    }
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int CurrentId
    {
        get => int.TryParse(idBox.Text, out var id) ? id : 0;
        set
        {
            idBox.Text = value > 0 ? value.ToString() : "Uus";
            deleteButton.Enabled = !busy && value > 0;
        }
    }
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string CurrentNimi { get => nameBox.Text; set => nameBox.Text = value; }
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string CurrentTuup { get => typeBox.Text; set => typeBox.Text = value; }
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string CurrentKirjeldus { get => descriptionBox.Text; set => descriptionBox.Text = value; }
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public decimal CurrentAlkoholiprotsent
    {
        get => alcoholBox.Value;
        set => alcoholBox.Value = Math.Clamp(value, alcoholBox.Minimum, alcoholBox.Maximum);
    }
    private void SelectionChanged(object sender, EventArgs e) => presenter?.SetSelection(SelectedItem);
    private void AddButton_Click(object sender, EventArgs e)
    {
        presenter.Add();
        nameBox.Focus();
        statusLabel.Text = "Uus kirje — täida väljad ja vajuta Salvesta.";
    }
    private async void SaveButton_Click(object sender, EventArgs e) =>
        await RunAction(() => presenter.Save(lifetime.Token));
    private async void DeleteButton_Click(object sender, EventArgs e)
    {
        var previousStatus = statusLabel.Text;
        await RunAction(async () =>
        {
            if (!await presenter.Delete(lifetime.Token)) statusLabel.Text = previousStatus;
        });
    }
    public bool ConfirmDelete() => MessageBox.Show(this, $"Kustutada õlu \"{CurrentNimi}\"?",
        "Kinnita kustutamine", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
    // Adapted from the teacher's 20.03 ShowError: display both kinds of API errors.
    public void ShowError(string message, OperationResult result)
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
        deleteButton.Enabled = !value && CurrentId > 0;
    }
    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        base.OnFormClosing(e);
        if (!e.Cancel) lifetime.Cancel();
    }
}

