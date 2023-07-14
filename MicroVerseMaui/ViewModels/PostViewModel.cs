﻿using CommunityToolkit.Mvvm.Collections;
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
    //Call Get Posts from the Api Service, store in an ObservableCollection
    public partial class PostViewModel : BaseViewModel
    {
        ApiService getPosts;


        HttpResponseMessage response;
        [ObservableProperty]

        ProfileInfo _userProfile;

        public ObservableCollection<Post> Posts { get; } = new();
        public PostViewModel(ApiService getPosts)
        {
            Title = "MicroVerse";
            this.getPosts = getPosts;

        }

        [RelayCommand]
        async Task CreatePage()

        {
            await Shell.Current.GoToAsync($"//{nameof(CreatePostPage)}");


        }

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

                    if (DeviceInfo.Platform == DevicePlatform.Android)
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
