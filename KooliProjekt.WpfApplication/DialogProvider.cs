using System.Windows;
namespace KooliProjekt.WpfApplication;
public class DialogProvider : IDialogProvider
{
    public bool Confirm(string message) => MessageBox.Show(message, "Kinnitus", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
    public void ShowError(string error) => MessageBox.Show(error, "Viga", MessageBoxButton.OK, MessageBoxImage.Error);
}
