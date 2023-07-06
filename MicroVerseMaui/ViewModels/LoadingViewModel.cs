using CommunityToolkit.Mvvm.Input;
using MicroVerseMaui.Models;
using MicroVerseMaui.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroVerseMaui.ViewModels
{
    public partial class LoadingViewModel : BaseViewModel
    {
        public LoadingViewModel()
        {
        }

        [RelayCommand]
        // Check if a returning user or a new visitor
        async Task ReturningUserCheck()
        {
            string userDetailsStr = Preferences.Get(nameof(App.CurrentUser), "");
            // No data for current visitor, navigate to Login Page

            if (string.IsNullOrWhiteSpace(userDetailsStr))
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
            else
            {
                var UserToken = await SecureStorage.GetAsync(nameof(App.Token));


                var UserInfo = JsonConvert.DeserializeObject<UserInfo>(userDetailsStr);
                App.CurrentUser = UserInfo;
                App.Token = UserToken;
                AppShell.Current.FlyoutHeader = new FlyoutViewModel();
                // App already has  data for current user, navigate to Startpage
                await Shell.Current.GoToAsync($"//{nameof(StartPage)}");


            }
        }
    }
}
