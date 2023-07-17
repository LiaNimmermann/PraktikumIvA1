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
    // ViewModel for the Login Page.
    public partial class LoginPageViewModel : BaseViewModel
    {


        [ObservableProperty]
        private string _emailInput; // Accessed as EmailInput

        [ObservableProperty]
        private string _passwordInput;

        private readonly IApiLogin _apiLogin;


        // Initializes a new instance of the LoginPageViewModel class.
        // Input: apiLogin of type IApiLogin interface.
        public LoginPageViewModel(IApiLogin apiLogin)
        {


            _apiLogin = apiLogin;
        }

        // Performs the login operation over API, and by success saves auth token/user info
        [RelayCommand]
        async void Login()

        {
            if (!string.IsNullOrWhiteSpace(EmailInput) && !String.IsNullOrWhiteSpace(PasswordInput))
            {
                var currentUser = new UserInfo();
                var response = await _apiLogin.Authenticate(new LoginInfo
                {
                    Email = EmailInput,
                    Password = PasswordInput
                });

                if (response != null)
                {
                    if (Preferences.ContainsKey(nameof(App.CurrentUser)))
                    {
                        Preferences.Remove(nameof(App.CurrentUser));
                    }
                    // Save user token
                    App.Token = response.token;
                    currentUser.Email = EmailInput;
                    string currentUserStr = JsonConvert.SerializeObject(currentUser);
                    Preferences.Set(nameof(App.CurrentUser), currentUserStr);
                    App.CurrentUser = currentUser;
                    AppShell.Current.FlyoutHeader = new FlyoutViewModel();

                    await Shell.Current.GoToAsync($"//{nameof(StartPage)}");

                    // Clear login input fields
                    EmailInput = string.Empty;
                    PasswordInput = string.Empty;
                }
                else
                {
                    await AppShell.Current.DisplayAlert("Error", "User does not exit. Make sure Email and Password are correct", "Ok");

                }




            }

        }

    }
}


