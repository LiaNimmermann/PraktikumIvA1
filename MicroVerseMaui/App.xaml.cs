using MicroVerseMaui.Models;

namespace MicroVerseMaui;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}
}
