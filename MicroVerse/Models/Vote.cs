using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MicroVerse.Models
{
    public class Vote
    {
        public Vote(Guid postId, string userId, int upvote)
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
        public int Upvote { get; set; }
        [Required]
        public DateTime CreatedAt { get; } = DateTime.Now;
    }
}
