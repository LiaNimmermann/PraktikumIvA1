using MicroVerseMaui.Models;
using System;
using System.Net.Http.Json;

namespace MicroVerseMaui.Services
{
    // Get Api data from local host
    public class ApiService
    {
        public ApiService()
        {
        }
        List<Post> postsList = new();
        //Get list of all posts
        public async Task<List<Post>> GetPosts()
        {
            HttpResponseMessage response;

            // If android, use helper for https request
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                var devSslHelper = new DevHttpsConnectionHelper(sslPort: 7028);
                var http = devSslHelper.HttpClient;
                // DevServerRootUrl = Localhost
                response = await http.GetAsync(devSslHelper.DevServerRootUrl + "/api/Post/");
            }
            else // Windows
            {
                var httpClient = new HttpClient();
                var url = "https://localhost:7028/api/Post";
                response = await httpClient.GetAsync(url);
            }

            if (response.IsSuccessStatusCode)
            {
                postsList = await response.Content.ReadFromJsonAsync<List<Post>>();
            }
            return postsList.OrderByDescending(post => post.CreatedAt).ToList();
        }

    }
}
