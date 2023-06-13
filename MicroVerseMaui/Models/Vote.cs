using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroVerseMaui.Models
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
