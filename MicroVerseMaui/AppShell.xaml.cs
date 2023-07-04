using MicroVerseMaui.Models;
using MicroVerseMaui.ViewModels;
using MicroVerseMaui.Views;

namespace MicroVerseMaui;

public partial class AppShell : Shell
{

    public AppShell()
	{
		InitializeComponent();
        this.BindingContext = new StartViewModel();

        Routing.RegisterRoute(nameof(StartPage), typeof(StartPage));
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));


    }
}
