using MicroVerseMaui.ViewModels;

namespace MicroVerseMaui.Views;

public partial class StartPage : ContentPage
{

    // Initializes a new instance of the StartPage class.
    // Input: viewModel of type ProfileViewModel, to bind data to the StartPage.
    public StartPage(StartViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
		Title = "......";
	}
}