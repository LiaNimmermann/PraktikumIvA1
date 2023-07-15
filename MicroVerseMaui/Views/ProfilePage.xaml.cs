using MicroVerseMaui.ViewModels;

namespace MicroVerseMaui.Views;

public partial class ProfilePage : ContentPage
{

    // Initializes a new instance of the ProfilePage class.
    // Input: viewModel of type ProfileViewModel, to bind data to the ProfilePage.
    public ProfilePage(ProfileViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
        // Auto load command on page load
        Loaded += (s, e) =>
        {
            viewModel.ProfileViewCommand.Execute(null);
        };
    }

    // Passing args, invoked when navigated to.
    // Input: args of type NavigatedToEventArgs
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }


    
}
