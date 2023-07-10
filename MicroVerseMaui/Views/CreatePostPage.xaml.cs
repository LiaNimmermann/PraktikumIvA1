using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MicroVerseMaui.ViewModels;
using Newtonsoft.Json;
using System.Text;

namespace MicroVerseMaui.Views;

public partial class CreatePostPage : ContentPage
{
    public CreatePostPage(CreatePostViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }




}