using MicroVerse.Models;

namespace MicroVerse.ViewModels
{
    public class PostViewModel
    {
    
        public int Id { get; set; }
        public string Body { get; set; }
        public PostViewModel? ReactsTo { get; set; }
        public DateTime CreatedAt { get; set; }
        public string DisplayedName { get; set; }
        public string? AuthorImage { get; set; } //URL to image
        public string Username { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public int? Status { get; set; }

        public PostViewModel() { } 

        public PostViewModel(int id, string body, PostViewModel reactsTo, DateTime createdAt, string displayName, string authorImage, string username, int upvotes, int downvotes, int status)
        {
            Id = id;
            Body = body;
            ReactsTo = reactsTo;
            CreatedAt = createdAt;
            DisplayedName = displayName;
            AuthorImage = authorImage;
            Username = username;
            Upvotes = upvotes;
            Downvotes = downvotes;
            Status = status;
        }
        public PostViewModel(string body, PostViewModel reactsTo, DateTime createdAt, string displayName, string username, int upvotes, int downvotes)
        {
            Body = body;
            ReactsTo = reactsTo;
            CreatedAt = createdAt;
            DisplayedName = displayName;
            Username = username;
            Upvotes = upvotes;
            Downvotes = downvotes;
            AuthorImage = "https://picsum.photos/200/200";
        }

    }
}
