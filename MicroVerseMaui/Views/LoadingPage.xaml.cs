using MicroVerseMaui.ViewModels;

namespace MicroVerseMaui.Views;

public partial class LoadingPage : ContentPage
{
	public LoadingPage(LoadingViewModel viewmodel)
	{
		InitializeComponent();
		this.BindingContext = viewmodel;
	}
}