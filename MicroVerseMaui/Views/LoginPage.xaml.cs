using MicroVerseMaui.ViewModels;

namespace MicroVerseMaui.Views;

public partial class LoginPage : ContentPage
{

    // Initializes a new instance of the LoginPage class.
    // Input: viewModel of type LoginPageViewModel, to bind data to the LoginPage.
    public LoginPage(LoginPageViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
}