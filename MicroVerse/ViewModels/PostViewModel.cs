using MicroVerse.Models;

namespace MicroVerse.ViewModels
{
    // A view model for posts
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
        public Vote.Votes VoteByUser { get; set; } = Vote.Votes.Undefined; // vote by currently logged in user

        public PostViewModel() { }

        public PostViewModel(Post post, IEnumerable<User> users)
        {
            var user = users.FirstOrDefault(u => u.UserName == post.AuthorId);
            Id = post.Id;
            Body = post.Body;
            ReactsTo = post.ReactsTo != null
                ? new PostViewModel(post.ReactsTo, users)
                : null;
            CreatedAt = post.CreatedAt;
            DisplayedName = user?.DisplayedName ?? "";
            Username = post.AuthorId;
            Upvotes = post.Votes
                .Where(x => x.Upvote == Vote.Votes.Up)
                .Count();
            Downvotes = post.Votes
                .Where(x => x.Upvote == Vote.Votes.Down)
                .Count();
            AuthorImage = user?.Picture is null
                ? "https://picsum.photos/200/200"
                : user.Picture;
            Status = ((int)post.Activation);
        }

    }
}
