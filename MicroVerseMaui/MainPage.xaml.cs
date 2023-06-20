using MicroVerseMaui.Models;
using MicroVerseMaui.Views;

namespace MicroVerseMaui;

public partial class MainPage : ContentPage
{
    static readonly HttpClient client = new HttpClient();

    public MainPage()
    {
        InitializeComponent();
        try
        {
            using var response = Task.Run(async () => await client.GetAsync("https://localhost:7028/api/Post")).Result;
            response.EnsureSuccessStatusCode();
            string responseBody = Task.Run(async () => await response.Content.ReadAsStringAsync()).Result;
            // Above three lines can be replaced with new helper method below
            // string responseBody = await client.GetStringAsync(uri);
            var posts = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<Post>>(responseBody);

            posts
                .OrderByDescending(x => x.CreatedAt)
                .ToList()
                .ForEach(
                    post =>
                    {
                        var label = new PostView(post);
                        VLayout.Add(label);
                        (VLayout as IView).InvalidateArrange();
                    }
                );
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", ex.Message);
        }
    }
}

