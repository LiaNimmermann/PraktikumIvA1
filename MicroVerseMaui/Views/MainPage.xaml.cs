using MicroVerseMaui.Models;
using MicroVerseMaui.ViewModels;


namespace MicroVerseMaui.Views;

public partial class MainPage : ContentPage
{
    // Initializes a new instance of the MainPage class.
    // Input: viewModel of type PostViewModel, to bind data to the MainPage.
    public MainPage(PostViewModel viewModel)
    {
        InitializeComponent();
        // Auto run command on page load
        BindingContext = viewModel;
        Loaded += (s, e) =>
        {
            viewModel.GetPostsCommand.Execute(null);
        };


    }

}

