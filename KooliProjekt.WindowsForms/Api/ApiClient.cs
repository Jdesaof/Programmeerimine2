using System.Net.Http.Json;
namespace KooliProjekt.WindowsForms.Api;

public sealed class ApiClient : IApiClient
{
    private readonly HttpClient client;
    public ApiClient(HttpClient client) { this.client = client ?? throw new ArgumentNullException(nameof(client)); }

    public async Task<PagedResult<Olu>> List(int page, int pageSize, CancellationToken token = default)
    {
        using var response = await client.GetAsync($"api/Olud?Page={page}&PageSize={pageSize}", token);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<OperationResult<PagedResult<Olu>>>(cancellationToken: token);
        return result?.Value ?? throw new InvalidOperationException("API ei tagastanud loendit.");
    }
    public async Task<Olu> Save(Olu item, CancellationToken token = default)
    {
        using var response = await client.PostAsJsonAsync("api/Olud", item, token);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<OperationResult<Olu>>(cancellationToken: token);
        return result?.Value ?? throw new InvalidOperationException("API ei tagastanud salvestatud kirjet.");
    }
    public async Task Delete(int id, CancellationToken token = default)
    {
        using var response = await client.DeleteAsync($"api/Olud/{id}", token);
        response.EnsureSuccessStatusCode();
    }
}

