using CommunityToolkit.Mvvm.Input;
using MicroVerseMaui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroVerseMaui.ViewModels 
{
    public partial class StartViewModel : BaseViewModel
    {
        [RelayCommand]
        async void LogOut()
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }

    }
}
