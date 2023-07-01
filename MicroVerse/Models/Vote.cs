using System.ComponentModel.DataAnnotations;

namespace MicroVerse.Models
{
    public class Vote
    {
        public enum Votes
        {
            Up = 1,
            Down = -1
        }

        public Vote(Guid postId, string userId, Votes upvote)
        {
            PostId = postId;
            UserId = userId;
            Upvote = upvote;
            CreatedAt = DateTime.UtcNow;
        }

        [Required]
        public Guid PostId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public Votes Upvote { get; set; }
        [Required]
        public DateTime CreatedAt { get; } = DateTime.Now;
    }
}
