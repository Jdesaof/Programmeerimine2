using System.Windows;
namespace KooliProjekt.WpfApplication;
public partial class MainWindow : Window
{
    private readonly MainWindowViewModel viewModel;
    public MainWindow()
    {
        InitializeComponent();
        viewModel = new MainWindowViewModel();
        DataContext = viewModel;
        Loaded += LoadWindow;
        Closed += (_, _) => viewModel.CancelPendingOperations();
    }
    private async void LoadWindow(object sender, RoutedEventArgs e)
    {
        Loaded -= LoadWindow;
        await viewModel.InitializeAsync();
    }
}
