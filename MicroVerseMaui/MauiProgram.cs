using Microsoft.Maui.LifecycleEvents;
using MicroVerseMaui.Services;
using MicroVerseMaui.ViewModels;
using Plugin.Firebase.Auth;
using Plugin.Firebase.Bundled.Shared;
using Plugin.Firebase.Crashlytics;
#if IOS
using Plugin.Firebase.Bundled.Platforms.iOS;
#else
using Plugin.Firebase.Bundled.Platforms.Android;
#endif

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


        builder.Services.AddTransient<Views.Profile>();
        builder.Services.AddSingleton<Views.CreatePostPage>();
        builder.Services.AddSingleton<Views.MainPage>();
        builder.Services.AddSingleton<Views.LoginPage>();
        builder.Services.AddSingleton<Views.StartPage>();
        builder.Services.AddSingleton<Views.LoadingPage>();


        builder.Services.AddSingleton<ApiService>();



        return builder.Build();
    }

    private static MauiAppBuilder RegisterFirebase(this MauiAppBuilder builder)
    {
        builder.ConfigureLifecycleEvents(events =>
        {
#if IOS
            events.AddiOS(iOS => iOS.FinishedLaunching((app, launchOptions) => {
                Firebase.Core.App.Configure();
                return false;
            }));
#else
            events.AddAndroid(android => android.OnCreate((activity, bundle) => {
                Firebase.FirebaseApp.InitializeApp(activity);
            }));
#endif
        });

        return builder;
    }

    private static CrossFirebaseSettings CreateCrossFirebaseSettings()
    {

        return new CrossFirebaseSettings(
         isAuthEnabled: true,
         isCloudMessagingEnabled: true,
         isAnalyticsEnabled: true);

    }
}

