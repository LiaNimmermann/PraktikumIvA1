namespace MicroVerseMaui.ViewModels;

public partial class FlyoutViewModel : StackLayout
{
    // add name of logged in user to Flyoutheader
	public FlyoutViewModel()
	{
		InitializeComponent();


        if (App.CurrentUser != null)
        {
            fEmail.Text = App.CurrentUser.Email;


        }
    }

}