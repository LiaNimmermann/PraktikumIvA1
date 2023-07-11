using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MicroVerseMaui.Models;
using MicroVerseMaui.Services;
using MicroVerseMaui.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroVerseMaui.ViewModels
{
    public partial class CreatePostViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string _newPostInput; // Accessed as NewPostInput


        [RelayCommand]
        async Task CreatePost()

        {
            var newPost = new NewPost(NewPostInput);

            HttpResponseMessage response;
            string postStr = JsonConvert.SerializeObject(newPost);

            // If android, use helper for https request
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                var devSslHelper = new DevHttpsConnectionHelper(sslPort: 7028);
                var http = devSslHelper.HttpClient;
                http.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.Token);
                response = await http.PostAsync(devSslHelper.DevServerRootUrl + "/api/Post/Authpost/",
                    new StringContent(postStr, Encoding.UTF8, "application/json"));
            }

            else // Windows
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.Token);
                var url = "https://localhost:7028/api/Post/AuthPost";
                response = await httpClient.PostAsync(url,
                new StringContent(postStr, Encoding.UTF8, "application/json"));
            }


            if (response.IsSuccessStatusCode)
            {
                await AppShell.Current.DisplayAlert("Success", "Post created successfully!" , "Ok");

                await Shell.Current.GoToAsync($"//{nameof(MainPage)}");


            }
            else
            {
                await AppShell.Current.DisplayAlert("Error", "Post couldn't be created", "Ok");

            }


        }
        [RelayCommand]
        async Task Home()

        {
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");

        }
    }
}
