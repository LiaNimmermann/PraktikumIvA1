using MicroVerseMaui.Models;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;


namespace MicroVerseMaui.Services
{
    // Get Api data from local host
    public class ApiService
    {
        HttpClient httpClient;

        public ApiService()
        {
            httpClient = new HttpClient();
        }
        List<Post> postsList = new();
        //Get list of all posts
        public async Task<List<Post>> GetPosts()
        {
            var url = "https://localhost:7028/api/Post";
            var response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                postsList = await response.Content.ReadFromJsonAsync<List<Post>>();
            }
            return postsList
                  .OrderByDescending(post => post.CreatedAt).ToList();
;
        }

    }
}
