using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KooliProjekt.BlazorWasm.Api;

public sealed class ApiClient : IApiClient
{
    private readonly HttpClient client;
    public ApiClient(HttpClient client) { this.client = client ?? throw new ArgumentNullException(nameof(client)); }

    public async Task<OperationResult<Olu>> Get(int id, CancellationToken token = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"api/Olud/{id}");
        var result = await Send<OperationResult<Olu>>(request, token);
        if (!result.HasErrors && result.Value == null) result.AddError("Kirjet ei leitud.");
        return result;
    }
    public async Task<OperationResult<PagedResult<Olu>>> List(int page, int pageSize, CancellationToken token = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"api/Olud?Page={page}&PageSize={pageSize}");
        var result = await Send<OperationResult<PagedResult<Olu>>>(request, token);
        if (!result.HasErrors && (result.Value == null || result.Value.Results == null))
            result.AddError("API ei tagastanud loendit.");
        return result;
    }
    public async Task<OperationResult<Olu>> Save(Olu item, CancellationToken token = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "api/Olud")
        {
            Content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json")
        };
        var result = await Send<OperationResult<Olu>>(request, token);
        if (!result.HasErrors && result.Value == null) result.AddError("API ei tagastanud salvestatud kirjet.");
        return result;
    }
    public async Task<OperationResult> Delete(int id, CancellationToken token = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Delete, $"api/Olud/{id}");
        return await Send<OperationResult>(request, token, allowEmpty: true);
    }
    // Read the body before HTTP status so server validation messages are preserved.
    private async Task<T> Send<T>(HttpRequestMessage request, CancellationToken token, bool allowEmpty = false)
        where T : OperationResult, new()
    {
        try
        {
            using var response = await client.SendAsync(request, token);
            var body = await response.Content.ReadAsStringAsync(token);
            var result = new T();
            if (!string.IsNullOrWhiteSpace(body))
            {
                try
                {
                    var json = JObject.Parse(body);
                    // Model binding can return ASP.NET ProblemDetails instead of OperationResult.
                    if (json.GetValue("errors", StringComparison.OrdinalIgnoreCase) is JObject fields)
                    {
                        foreach (var field in fields.Properties())
                        {
                            var message = field.Value is JArray values
                                ? string.Join(Environment.NewLine, values.Values<string>()) : field.Value.ToString();
                            result.AddPropertyError(field.Name, message);
                        }
                    }
                    else result = JsonConvert.DeserializeObject<T>(body) ?? new T();
                }
                catch (JsonException)
                {
                    result.AddError($"API tagastas vigase vastuse (HTTP {(int)response.StatusCode}).");
                }
            }
            else if (response.IsSuccessStatusCode && !allowEmpty) result.AddError("API tagastas tühja vastuse.");
            if (!response.IsSuccessStatusCode && !result.HasErrors)
                result.AddError(response.StatusCode == HttpStatusCode.NotFound
                    ? "Kirjet ei leitud (404). Värskenda nimekirja."
                    : $"API päring ebaõnnestus (HTTP {(int)response.StatusCode}).");
            return result;
        }
        catch (HttpRequestException)
        {
            var result = new T();
            result.AddError("API-ga ei saanud ühendust. Kontrolli, et WebAPI töötab aadressil http://localhost:5086.");
            return result;
        }
        catch (OperationCanceledException) when (!token.IsCancellationRequested)
        {
            var result = new T();
            result.AddError("API vastuse ooteaeg sai läbi. Proovi uuesti.");
            return result;
        }
    }
}

