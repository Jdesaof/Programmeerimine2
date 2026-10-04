using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using KooliProjekt.BlazorWasm;
using KooliProjekt.BlazorWasm.Api;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri("http://localhost:5086/"), Timeout = TimeSpan.FromSeconds(30) });
builder.Services.AddScoped<IApiClient, ApiClient>();
await builder.Build().RunAsync();