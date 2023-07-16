using System.ComponentModel.DataAnnotations;

namespace MicroVerse.Models
{
    // a vote on a post
    public class Vote
    {
        // the possible values a vote can take
        public enum Votes
        {
            Up = 1,
            Down = -1,
            Undefined = 0
        }

        public Vote(Guid postId, string userId, Votes upvote)
        {
            PostId = postId;
            UserId = userId;
            Upvote = upvote;
            CreatedAt = DateTime.UtcNow;
        }

        // the post that is being voted on
        [Required]
        public Guid PostId { get; set; }

        // the user that made the vote
        [Required]
        public string UserId { get; set; }

        [Required]
        public Votes Upvote { get; set; }

        [Required]
        public DateTime CreatedAt { get; } = DateTime.Now;
    }
}
