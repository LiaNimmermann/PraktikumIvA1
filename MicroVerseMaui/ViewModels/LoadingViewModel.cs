using CommunityToolkit.Mvvm.Input;
using MicroVerseMaui.Models;
using MicroVerseMaui.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroVerseMaui.ViewModels
{
    // ViewModel for the loading screen.
    public partial class LoadingViewModel : BaseViewModel
    {
        public LoadingViewModel()
        {
        }

        // Check if a returning user or a new visitor
        [RelayCommand]
         async Task ReturningUserCheck()
        {
            string userDetailsStr = Preferences.Get(nameof(App.CurrentUser), "");
            // No active session for current visitor, navigate to Login Page

            if (string.IsNullOrWhiteSpace(userDetailsStr))
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
            else
            {
                // App already has  data for current user, navigate to Startpage

                var UserToken = await SecureStorage.GetAsync(nameof(App.Token));
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(UserToken) as JwtSecurityToken;

                // Ensure authentication token is still valid
                if (jsonToken.ValidTo < DateTime.UtcNow) { // = Token expired
                    await Shell.Current.DisplayAlert("The user session has expired", "Please login again", "OK");
                    await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                }
                else
                {
                    var UserInfo = JsonConvert.DeserializeObject<UserInfo>(userDetailsStr);
                    App.CurrentUser = UserInfo;
                    App.Token = UserToken;
                    AppShell.Current.FlyoutHeader = new FlyoutViewModel();
                    await Shell.Current.GoToAsync($"//{nameof(StartPage)}");
                }




            }
        }
    }
}
