using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MicroVerseMaui.Models;
using MicroVerseMaui.Services;
using MicroVerseMaui.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

//ViewModel for own Profile Page.

namespace MicroVerseMaui.ViewModels
{
    public partial class MyProfileViewModel : BaseViewModel
    {
        public MyProfileViewModel() { 
        }

        [ObservableProperty]
        Post post;

        [ObservableProperty]

        ProfileInfo _userProfile;
        // Gets the collection of posts by the user.
        public ObservableCollection<Post> UserPosts { get; } = new();


        [ObservableProperty]

        List<Post> _getUserPosts;

        [RelayCommand]
        async Task MyProfileView()

        {
            HttpResponseMessage profileResponse;
            HttpResponseMessage postsResponse;
            // Get user info for the loggen in user, using user email
            if (DeviceInfo.Platform == DevicePlatform.Android)  // Use Helper 
            {
                var devSslHelper = new DevHttpsConnectionHelper(sslPort: 7028);
                var httpClient = devSslHelper.HttpClient;
                var url = $"/api/User/GetuserEmail?Email={App.CurrentUser.Email}";

                profileResponse = await httpClient.GetAsync(devSslHelper.DevServerRootUrl + url);

            }

            else
            {
                var httpClient = new HttpClient();
                var url = $"https://localhost:7028/api/User/GetuserEmail?Email={App.CurrentUser.Email}";
                profileResponse = await httpClient.GetAsync(url);
            }
            UserProfile = await profileResponse.Content.ReadFromJsonAsync<ProfileInfo>();

            // Get all posts by the user
            if (DeviceInfo.Platform == DevicePlatform.Android) //  Use Helper 
            {
                var devSslHelper = new DevHttpsConnectionHelper(sslPort: 7028);
                var httpClient = devSslHelper.HttpClient;
                var url2 = $"/api/Post/fromuser?userName={UserProfile.userName}";

                postsResponse = await httpClient.GetAsync(devSslHelper.DevServerRootUrl + url2);

            }
            else
            {
                var httpClient2 = new HttpClient();
                var url2 = $"https://localhost:7028/api/Post/fromuser?userName={UserProfile.userName}";
                postsResponse = await httpClient2.GetAsync(url2);


            }


            GetUserPosts = await postsResponse.Content.ReadFromJsonAsync<List<Post>>();
            foreach (var post in GetUserPosts)
            {

                if (DeviceInfo.Platform == DevicePlatform.Android) //  Use Helper 
                {
                    var devSslHelper = new DevHttpsConnectionHelper(sslPort: 7028);
                    var httpClient = devSslHelper.HttpClient;
                    var url = $"/api/User/{post.AuthorId}";

                    profileResponse = await httpClient.GetAsync(devSslHelper.DevServerRootUrl + url);

                }

                else
                {
                    var httpClient = new HttpClient();
                    var url = $"https://localhost:7028/api/User/{post.AuthorId}";
                    profileResponse = await httpClient.GetAsync(url);
                }


                UserProfile = await profileResponse.Content.ReadFromJsonAsync<ProfileInfo>();
                post.Picture = UserProfile.Picture;
                post.displayedName = UserProfile.displayedName;

                UserPosts.Add(post);
            }
        }
        // Navigate to Homepage
        [RelayCommand]
        async Task HomePage()

        {
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");

        }

    }
    
}