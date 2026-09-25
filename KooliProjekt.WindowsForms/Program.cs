using KooliProjekt.WindowsForms.Api;
namespace KooliProjekt.WindowsForms;
internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        using var httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5086/"),
            Timeout = TimeSpan.FromSeconds(15)
        };
        IApiClient apiClient = new ApiClient(httpClient);
        Application.Run(new Form1(apiClient));
    }
}

