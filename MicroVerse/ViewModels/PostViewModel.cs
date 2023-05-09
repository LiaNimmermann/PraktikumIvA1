using MicroVerse.Models;

namespace MicroVerse.ViewModels
{
    public class PostViewModel
    {
    
        public int Id { get; set; }
        public string Body { get; set; }
        public UserModel Author { get; set; }
        public PostViewModel ReactsTo { get; set; }
        public DateTime CreatedAt { get; set; }
        public string DisplayName { get; set; }
        public byte[] AuthorImage { get; set; }
        public string Username { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public int Status { get; set; }

        public PostViewModel(int id, string body, UserModel author, PostViewModel reactsTo, DateTime createdAt, string displayName, byte[] authorImage, string username, int upvotes, int downvotes, int status)
        {
            Id = id;
            Body = body;
            Author = author;
            ReactsTo = reactsTo;
            CreatedAt = createdAt;
            DisplayName = displayName;
            AuthorImage = authorImage;
            Username = username;
            Upvotes = upvotes;
            Downvotes = downvotes;
            Status = status;
        }

    }
}
