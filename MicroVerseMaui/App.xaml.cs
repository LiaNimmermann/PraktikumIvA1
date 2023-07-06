using MicroVerseMaui.Models;

namespace MicroVerseMaui;

public partial class App : Application
{
    public static UserInfo CurrentUser;
    public static string Token;


    public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}
}
