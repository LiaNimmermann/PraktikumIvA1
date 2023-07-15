namespace MicroVerseMaui.ViewModels;


// ViewModel for the Flyout header.

public partial class FlyoutViewModel : StackLayout
{
    // Add info of logged in user to Flyoutheader
	public FlyoutViewModel()
	{
		InitializeComponent();


        if (App.CurrentUser != null)
        {
            fEmail.Text = App.CurrentUser.Email;


        }
    }

}