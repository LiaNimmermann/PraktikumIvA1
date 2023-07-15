using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MicroVerseMaui.ViewModels;
using Newtonsoft.Json;
using System.Text;

namespace MicroVerseMaui.Views;

public partial class CreatePostPage : ContentPage
{
    // Initializes a new instance of the CreatePostPage class.
    // Input: viewModel of type CreatePostViewModel, to bind data to the CreatePostPage.
    public CreatePostPage(CreatePostViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }




}