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
            // Remove user date from APP
            if (Preferences.ContainsKey(nameof(App.CurrentUser)))
            {
                Preferences.Remove(nameof(App.CurrentUser));
            }
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }

    }
}
