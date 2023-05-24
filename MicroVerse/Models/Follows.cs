using System.ComponentModel.DataAnnotations;

namespace MicroVerse.Models
{
    public class Follows
    {
        public Follows(string followingUserId, string followedUserId)
        {
            FollowingUserId = followingUserId;
            FollowedUserId = followedUserId;
            CreatedAt = DateTime.Now;
        }

        [Required]
        public string FollowingUserId { get; set; } = "";

        [Required]
        public string FollowedUserId { get; set; } = "";

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
