using MicroVerseMaui.ViewModels;

namespace MicroVerseMaui.Views;

public partial class LoadingPage : ContentPage
{

    // Initializes a new instance of the LoadingPage class.
    // Input: viewModel of type LoadingViewModel, to bind data to the LoadingPage.
    public LoadingPage(LoadingViewModel viewmodel)
	{
		this.BindingContext = viewmodel;
        InitializeComponent();
        // Auto run command by page load
        Loaded += (s, e) =>
        {
            viewmodel.ReturningUserCheckCommand.Execute(null);
        };
    }
}