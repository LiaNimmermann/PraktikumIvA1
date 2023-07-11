using MicroVerseMaui.ViewModels;

namespace MicroVerseMaui.Views;

public partial class Profile : ContentPage
{
    public Profile(ProfileViewModel viewModel)
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
