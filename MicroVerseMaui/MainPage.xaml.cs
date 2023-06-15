using MicroVerseMaui.Models;

namespace MicroVerseMaui;

public partial class MainPage : ContentPage
{
    static readonly HttpClient client = new HttpClient();

    public MainPage()
	{
		InitializeComponent();
    }

	private void OnCounterClicked(object sender, EventArgs e)
	{
        try
        {
            using var response = Task.Run(async () => await client.GetAsync("https://localhost:7028/api/Post")).Result;
            response.EnsureSuccessStatusCode();
            string responseBody = Task.Run(async () => await response.Content.ReadAsStringAsync()).Result;
            // Above three lines can be replaced with new helper method below
            // string responseBody = await client.GetStringAsync(uri);
            var posts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Post>>(responseBody);

            posts.ForEach(
                post =>
                {
                    var label = new Label() { Text = post.Body };
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

