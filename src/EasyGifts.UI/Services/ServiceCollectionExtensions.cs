using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace EasyGifts.UI.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEasyGiftsServices(this IServiceCollection services, string apiBaseUrl)
    {
        services.AddBlazoredLocalStorage();

        services.AddHttpClient<ApiClient>(client =>
        {
            client.BaseAddress = new Uri(apiBaseUrl);
        });

        services.AddScoped<IAuthService>(sp => sp.GetRequiredService<ApiClient>());
        services.AddScoped<IGiftService>(sp => sp.GetRequiredService<ApiClient>());
        services.AddScoped<IGroupService>(sp => sp.GetRequiredService<ApiClient>());

        services.AddScoped<JwtAuthenticationStateProvider>();
        services.AddScoped<AuthenticationStateProvider>(sp =>
            sp.GetRequiredService<JwtAuthenticationStateProvider>());

        services.AddAuthorizationCore();

        return services;
    }
}
