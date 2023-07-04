using MicroVerseMaui.ViewModels;

namespace MicroVerseMaui.Views;

public partial class StartPage : ContentPage
{
	public StartPage(StartViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
		Title = "......";
	}
}