using MicroVerseMaui.Models;

namespace MicroVerseMaui;

public partial class App : Application
{
    public static UserInfo CurrentUser;

    public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}
}
