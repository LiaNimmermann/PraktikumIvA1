using MicroVerseMaui.Models;
using MicroVerseMaui.ViewModels;

namespace MicroVerseMaui.Views;

public partial class MainPage : ContentPage
{
    public MainPage(PostViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

    }

}

