using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MicroVerse.Models
{
    public class Vote
    {
        [Required]
        public int PostId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int Upvote { get; set; }
        [Required]
        public DateTime CreatedAt { get; } = DateTime.Now;
    }
}
