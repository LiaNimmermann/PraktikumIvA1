using MicroVerseMaui.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http.Json;
using System.Text;

namespace MicroVerseMaui.Services
{
    // Class that handles getting Posts from API
    public class ApiGetPosts
    {
        public ApiGetPosts()
        {
        }
        List<Post> postsList = new();
        // Gets list of all posts
        // Return: List of posts ordered by most recent
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
