using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MicroVerseMaui.Models;
using MicroVerseMaui.Services;
using MicroVerseMaui.Views;
using Newtonsoft.Json;

namespace MicroVerseMaui.ViewModels
{
    public partial class LoginPageViewModel : BaseViewModel
    {


        [ObservableProperty]
        private string _email; // Accessed as Email

        [ObservableProperty]
        private string _password;


        [RelayCommand]

        void Login()
                            
        {
            if (!string.IsNullOrWhiteSpace(Email) && !String.IsNullOrWhiteSpace(Password))
            {
                var currentUser = new UserInfo()
 
                {
                    UserName = "",
                    Email = Email
                };
                if (Preferences.ContainsKey(nameof(App.CurrentUser)))
                {
                    Preferences.Remove(nameof(App.CurrentUser));
                }
                string currentUserStr = JsonConvert.SerializeObject(currentUser);
                Preferences.Set(nameof(App.CurrentUser), currentUserStr);
                App.CurrentUser = currentUser;
                AppShell.Current.FlyoutHeader = new FlyoutViewModel();

                Shell.Current.GoToAsync($"//{nameof(StartPage)}");

            }

        }

    }
}
    

