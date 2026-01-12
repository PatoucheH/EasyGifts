using Microsoft.Extensions.Logging;
using EasyGifts.UI.Services;

namespace EasyGifts.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();

        // API Base URL - adjust for your environment
        // Use 10.0.2.2 for Android emulator to reach localhost
        var apiBaseUrl = DeviceInfo.Platform == DevicePlatform.Android
            ? "http://10.0.2.2:5000/"
            : "https://localhost:5001/";

        builder.Services.AddEasyGiftsServices(apiBaseUrl);

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
