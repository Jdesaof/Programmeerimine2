using KooliProjekt.WindowsForms.Api;

namespace KooliProjekt.WindowsForms;

public class MainViewPresenter
{
    private readonly IApiClient apiClient;
    private readonly IMainView view;

    public MainViewPresenter(IApiClient apiClient, IMainView view)
    {
        this.apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        this.view = view ?? throw new ArgumentNullException(nameof(view));
        view.SetPresenter(this);
    }

    public async Task LoadData(int selectId = 0, CancellationToken token = default)
    {
        var items = new List<Olu>();
        var page = 1;
        PagedResult<Olu> result;
        do
        {
            token.ThrowIfCancellationRequested();
            var response = await apiClient.List(page, 100, token);
            token.ThrowIfCancellationRequested();
            if (response.HasErrors)
            {
                // Keep the existing list and editor contents when refreshing fails.
                view.ShowError("Viga andmete laadimisel", response);
                return;
            }
            result = response.Value;
            items.AddRange(result.Results);
            page++;
        } while (page <= result.PageCount);

        view.DataSource = items;
        var selected = items.FirstOrDefault(item => item.Id == selectId);
        view.SelectedItem = selected;
        SetSelection(selected);
    }

    public void SetSelection(Olu item)
    {
        view.CurrentId = item?.Id ?? 0;
        view.CurrentNimi = item?.Nimi ?? "";
        view.CurrentTuup = item?.Tuup ?? "";
        view.CurrentKirjeldus = item?.Kirjeldus ?? "";
        view.CurrentAlkoholiprotsent = item?.Alkoholiprotsent ?? 0m;
    }

    public void Add()
    {
        view.SelectedItem = null;
        SetSelection(null);
    }

    public async Task Save(CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();
        var item = new Olu
        {
            Id = view.CurrentId, Nimi = view.CurrentNimi, Tuup = view.CurrentTuup,
            Kirjeldus = view.CurrentKirjeldus, Alkoholiprotsent = view.CurrentAlkoholiprotsent
        };
        var result = await apiClient.Save(item, token);
        token.ThrowIfCancellationRequested();
        if (result.HasErrors)
        {
            view.ShowError("Viga salvestamisel", result);
            return;
        }
        await LoadData(result.Value.Id, token);
    }

    // False means no deletion was attempted; the view can keep its previous status.
    public async Task<bool> Delete(CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();
        var id = view.CurrentId;
        if (id <= 0 || !view.ConfirmDelete()) return false;
        var result = await apiClient.Delete(id, token);
        token.ThrowIfCancellationRequested();
        if (result.HasErrors)
        {
            view.ShowError("Viga kustutamisel", result);
            return true;
        }
        await LoadData(token: token);
        return true;
    }
}