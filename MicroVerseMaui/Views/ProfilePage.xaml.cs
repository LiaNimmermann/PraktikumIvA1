using MicroVerseMaui.ViewModels;

namespace MicroVerseMaui.Views;

public partial class ProfilePage : ContentPage
{
    public ProfilePage(ProfileViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
        Loaded += (s, e) =>
        {
            viewModel.ProfileViewCommand.Execute(null);
        };
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }


    
}
