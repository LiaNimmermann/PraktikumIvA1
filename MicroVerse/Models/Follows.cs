using System.ComponentModel.DataAnnotations;

namespace MicroVerse.Models
{
    public class Follows
    {
        public Follows() { }

        [Required]
        public String FollowingUserId { get; set; } = "";

        [Required]
        public String FollowedUserId { get; set; } = "";

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
