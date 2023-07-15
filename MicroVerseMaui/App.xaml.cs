using MicroVerseMaui.Models;

namespace MicroVerseMaui;

public partial class App : Application
{
    public static UserInfo CurrentUser; // Info for current logged in user
    public static string Token; // Auth token for current logged in user


    public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}
}
