using MicroVerseMaui.ViewModels;

namespace MicroVerseMaui.Views;

public partial class MyProfile : ContentPage
{
    // Initializes a new instance of the MyProfile class.
    // Input: viewModel of type MyProfileViewModel, to bind data to MyProfile page.
    public MyProfile(MyProfileViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
        // Auto load command on page load
        Loaded += (s, e) =>
        {
            viewModel.MyProfileViewCommand.Execute(null);
        };

    }
}
