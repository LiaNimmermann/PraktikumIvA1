using CommunityToolkit.Mvvm.Input;
using MicroVerseMaui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroVerseMaui.ViewModels 
{
    // ViewModel for the Start Page.
    public partial class StartViewModel : BaseViewModel
    {

        // Performs the logout operation and navigate to login page.
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
