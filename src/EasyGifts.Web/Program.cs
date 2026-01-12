using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using EasyGifts.Web;
using EasyGifts.UI.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// API Base URL - adjust for your environment
var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "https://localhost:5001/";
builder.Services.AddEasyGiftsServices(apiBaseUrl);

await builder.Build().RunAsync();
