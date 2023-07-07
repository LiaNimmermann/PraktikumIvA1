using MicroVerse.Models;

namespace MicroVerse.ViewModels
{
    public class PostViewModel
    {
        public Guid Id { get; set; }
        public string Body { get; set; } = String.Empty;
        public PostViewModel? ReactsTo { get; set; }
        public DateTime CreatedAt { get; set; }
        public string DisplayedName { get; set; } = String.Empty;
        public string? AuthorImage { get; set; } //URL to image
        public string Username { get; set; } = String.Empty;
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public int? Status { get; set; }
        public int VoteByUser { get; set; } = 0; //-1 for downvote, 1 for upvote by currently logged in user

        public PostViewModel() { }

        public PostViewModel(Guid id, string body, PostViewModel reactsTo, DateTime createdAt, string displayName, string authorImage, string username, int upvotes, int downvotes, int status)
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

        public PostViewModel(Guid id, string body, PostViewModel? reactsTo, DateTime createdAt, string displayName, string username, int upvotes, int downvotes)
        {
            Id = id;
            Body = body;
            ReactsTo = reactsTo;
            CreatedAt = createdAt;
            DisplayedName = displayName;
            Username = username;
            Upvotes = upvotes;
            Downvotes = downvotes;
            AuthorImage = "https://picsum.photos/200/200";
        }

        public PostViewModel(Post post, IEnumerable<User> users)
        {
            Id = post.Id;
            Body = post.Body;
            ReactsTo = post.ReactsTo != null
                ? new PostViewModel(post.ReactsTo, users)
                : null;
            CreatedAt = post.CreatedAt;
            DisplayedName = users
                .FirstOrDefault(u => u.UserName == post.AuthorId)
                .DisplayedName;
            Username = post.AuthorId;
            Upvotes = post.Votes
                .Where(x => x.Upvote == Vote.Votes.Up)
                .Count();
            Downvotes = post.Votes
                .Where(x => x.Upvote == Vote.Votes.Down)
                .Count();
            AuthorImage = "https://picsum.photos/200/200";
        }

    }
}
