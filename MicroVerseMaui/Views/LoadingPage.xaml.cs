using MicroVerseMaui.ViewModels;

namespace MicroVerseMaui.Views;

public partial class LoadingPage : ContentPage
{
	public LoadingPage(LoadingViewModel viewmodel)
	{
		InitializeComponent();
		this.BindingContext = viewmodel;
        InitializeComponent();
        // Auto run command
        Loaded += (s, e) =>
        {
            viewmodel.ReturningUserCheckCommand.Execute(null);
        };
    }
}