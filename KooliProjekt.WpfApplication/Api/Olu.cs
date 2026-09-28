namespace KooliProjekt.WpfApplication;
public class Olu : NotifyPropertyChangedBase
{
    private int _Id;
    public int Id { get => _Id; set { if (_Id == value) return; _Id = value; NotifyPropertyChanged(); } }
    private string _Nimi;
    public string Nimi { get => _Nimi; set { if (_Nimi == value) return; _Nimi = value; NotifyPropertyChanged(); } }
    private string _Tuup;
    public string Tuup { get => _Tuup; set { if (_Tuup == value) return; _Tuup = value; NotifyPropertyChanged(); } }
    private string _Kirjeldus;
    public string Kirjeldus { get => _Kirjeldus; set { if (_Kirjeldus == value) return; _Kirjeldus = value; NotifyPropertyChanged(); } }
    private decimal _Alkoholiprotsent;
    public decimal Alkoholiprotsent { get => _Alkoholiprotsent; set { if (_Alkoholiprotsent == value) return; _Alkoholiprotsent = value; NotifyPropertyChanged(); } }
}
