namespace KooliProjekt.WindowsForms;

public interface IMainView
{
    IList<Olu> DataSource { get; set; }
    Olu SelectedItem { get; set; }
    void SetPresenter(MainViewPresenter presenter);
    bool ConfirmDelete();
    void ShowError(string message, OperationResult result);
    int CurrentId { get; set; }
    string CurrentNimi { get; set; }
    string CurrentTuup { get; set; }
    string CurrentKirjeldus { get; set; }
    decimal CurrentAlkoholiprotsent { get; set; }
}