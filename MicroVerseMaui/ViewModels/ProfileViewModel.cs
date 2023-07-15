using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MicroVerseMaui.Models;
using MicroVerseMaui.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;


namespace MicroVerseMaui.ViewModels
{
    //ViewModel for the Profile Page.

    [QueryProperty("Post","Post")]
    public partial class ProfileViewModel : BaseViewModel
    {
        public ProfileViewModel()
        {



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

        // Gets user info for current profile page, including list of all posts by the author
        async Task ProfileView()

        {
            HttpResponseMessage profileResponse;
            HttpResponseMessage postsResponse;

            // Get user info for the profile page
            if (DeviceInfo.Platform == DevicePlatform.Android)  // Use Helper 
            {
                var devSslHelper = new DevHttpsConnectionHelper(sslPort: 7028);
                var httpClient = devSslHelper.HttpClient;
                var url = $"/api/User/{Post.AuthorId}";

                profileResponse = await httpClient.GetAsync(devSslHelper.DevServerRootUrl + url);

            }

            else
            {
                var httpClient = new HttpClient();
                var url = $"https://localhost:7028/api/User/{Post.AuthorId}";
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
    }
}
