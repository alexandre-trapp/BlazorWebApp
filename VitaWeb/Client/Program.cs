using VitaWeb.Client;
using VitaWeb.Shared;
using Blazored.LocalStorage;
using VitaWeb.Client.Handlers;
using VitaWeb.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

Environment.SetEnvironmentVariable("MFKSERVER_API_URI", "http://localhost:5000/api/");
Environment.SetEnvironmentVariable("MFKSECURITATEM_API_URI", "http://localhost:5002/api/mfksecuritatem/");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddTransient<ValidateHeaderHandler>();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddMemoryCache();

builder.Services.AddHttpClient<IAuthenticationService, AuthenticationService>();
builder.Services.AddHttpClient<ILicencasService, LicencasService>();
builder.Services.AddHttpClient<IUFStoresService<UF>, UFStoresService<UF>>();
builder.Services.AddHttpClient<IUFStoresService<UFCreate>, UFStoresService<UFCreate>>();

builder.Services.AddSingleton<HttpClient>();

builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
