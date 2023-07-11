using MicroVerseMaui.Models;
using MicroVerseMaui.ViewModels;
using Plugin.Firebase.CloudMessaging;

namespace MicroVerseMaui.Views;

public partial class MainPage : ContentPage
{
    public MainPage(PostViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        Loaded += (s, e) =>
        {
            viewModel.GetPostsCommand.Execute(null);
        };


    }
    private async void OnCounterClicked(object sender, EventArgs e)
    {
        await CrossFirebaseCloudMessaging.Current.CheckIfValidAsync();
        var token = await CrossFirebaseCloudMessaging.Current.GetTokenAsync();
        Console.WriteLine($"FCM token: {token}");
    }
}

