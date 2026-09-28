using System.Windows;
namespace KooliProjekt.WpfApplication;
public partial class MainWindow : Window
{
    private readonly CancellationTokenSource lifetime = new();
    private readonly MainWindowViewModel viewModel;
    public MainWindow()
    {
        InitializeComponent();
        viewModel = new MainWindowViewModel();
        DataContext = viewModel;
        Loaded += LoadWindow;
        Closed += (_, _) => lifetime.Cancel();
    }
    private async void LoadWindow(object sender, RoutedEventArgs e)
    {
        Loaded -= LoadWindow;
        try
        {
            var result = await viewModel.LoadDataAsync(lifetime.Token);
            if (lifetime.IsCancellationRequested || !result.HasErrors) return;
            var messages = new List<string>();
            if (result.Errors != null) messages.AddRange(result.Errors);
            if (result.PropertyErrors != null)
                messages.AddRange(result.PropertyErrors.Select(p => p.Key + ": " + p.Value));
            MessageBox.Show(this, string.Join(Environment.NewLine, messages), "Viga andmete laadimisel", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (OperationCanceledException) when (lifetime.IsCancellationRequested) { }
        catch (Exception error)
        {
            if (!lifetime.IsCancellationRequested)
                MessageBox.Show(this, error.Message, "Viga andmete laadimisel", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}