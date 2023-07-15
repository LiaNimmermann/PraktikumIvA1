using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using MicroVerseMaui.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroVerseMaui.Services;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using MicroVerseMaui.Views;
using System.Net.Http.Json;

namespace MicroVerseMaui.ViewModels
{
    // ViewModel for the Posts Page.
    public partial class PostViewModel : BaseViewModel
    {
        ApiGetPosts getPosts; // Get posts from API


        HttpResponseMessage response;
        [ObservableProperty]

        ProfileInfo _userProfile;

        public ObservableCollection<Post> Posts { get; } = new();

        // Initializes a new instance of the PostViewModel class.
        // Input: getPosts of type of ApiGetPosts class.
        public PostViewModel(ApiGetPosts getPosts)
        {
            Title = "MicroVerse";
            this.getPosts = getPosts;

        }
        //  Navigates to the CreatePostPage.
        [RelayCommand]
        async Task CreatePage()

        {
            await Shell.Current.GoToAsync($"//{nameof(CreatePostPage)}");


        }

        //  Opens the profileview page for author of selected post, while passing over parameter for clicked post info.
        // Input: currentPost of Type Post
        [RelayCommand]
        async Task OpenProfile(Post currentPost)
        {
            if (currentPost == null)
                return;

            await Shell.Current.GoToAsync(nameof(ProfilePage), true, new Dictionary<string, object>
        {
            {"Post", currentPost }
        });
        }

        // Gets list of all posts, and attach relevant profile info.

        [RelayCommand]
        async Task GetPostsAsync()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                var posts = await getPosts.GetPosts();
                if (Posts.Count != 0)
                    Posts.Clear();
                foreach (var post in posts) { 
                    // Use an API call to get relevant profile info for each post's Author
                    if (DeviceInfo.Platform == DevicePlatform.Android) // Use Helper if device is Android, for HTTPS requests over localhost
                    {
                        var devSslHelper = new DevHttpsConnectionHelper(sslPort: 7028);
                        var httpClient = devSslHelper.HttpClient;
                        var url = $"/api/User/{post.AuthorId}";

                        response = await httpClient.GetAsync(devSslHelper.DevServerRootUrl + url);

                    }

                    else
                    {
                        var httpClient = new HttpClient();
                        var url = $"https://localhost:7028/api/User/{post.AuthorId}";
                        response = await httpClient.GetAsync(url);
                    }


                UserProfile = await response.Content.ReadFromJsonAsync<ProfileInfo>();
                post.Picture = UserProfile.Picture;
                post.displayedName = UserProfile.displayedName;




                Posts.Add(post);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error",
                    $"Unable to get Posts: {ex.Message}", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
