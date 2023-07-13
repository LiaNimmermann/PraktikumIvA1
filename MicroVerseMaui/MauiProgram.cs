using Microsoft.Maui.LifecycleEvents;
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

        builder.Services.AddSingleton<IApiLogin, ApiLogin>();


        builder.Services.AddSingleton<PostViewModel>();
        builder.Services.AddSingleton<LoginPageViewModel>();
        builder.Services.AddSingleton<StartViewModel>();
        builder.Services.AddSingleton<LoadingViewModel>();
        builder.Services.AddSingleton<FlyoutViewModel>();
        builder.Services.AddSingleton<CreatePostViewModel>();
        builder.Services.AddTransient<ProfileViewModel>();


        builder.Services.AddTransient<Views.ProfilePage>();
        builder.Services.AddSingleton<Views.CreatePostPage>();
        builder.Services.AddSingleton<Views.MainPage>();
        builder.Services.AddSingleton<Views.LoginPage>();
        builder.Services.AddSingleton<Views.StartPage>();
        builder.Services.AddSingleton<Views.LoadingPage>();


        builder.Services.AddSingleton<ApiService>();



        return builder.Build();
    }

  

 
}

