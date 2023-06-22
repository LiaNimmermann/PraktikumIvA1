using MicroVerseMaui.Services;
using MicroVerseMaui.ViewModels;
namespace MicroVerseMaui;

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
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        builder.Services.AddSingleton<PostViewModel>();
        builder.Services.AddSingleton<Views.MainPage>();
        builder.Services.AddSingleton<ApiService>();



        return builder.Build();
    }
}
