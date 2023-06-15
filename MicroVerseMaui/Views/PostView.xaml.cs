using MicroVerseMaui.Models;

namespace MicroVerseMaui.Views;

public partial class PostView : ContentView
{
	public Post Post { get; set; }
	public PostView(Post post)
	{
        Post = post;
		InitializeComponent();

        content.SetBinding(Label.TextProperty, new Binding("Body", source: Post));
        author.SetBinding(Label.TextProperty, new Binding("AuthorId", source: Post));
    }
}