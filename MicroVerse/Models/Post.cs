using System.ComponentModel.DataAnnotations;

namespace MicroVerse.Models
{
    // the data class for a post
    public class Post
    {

        public Post(string body, string authorId)
        {
            Id = Guid.NewGuid();
            Body = body;
            AuthorId = authorId;
        }

        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Body { get; set; }

        [Required]
        public string AuthorId { get; set; }

        public Post? ReactsTo { get; set; }

        // The activation of the post (active, blocked, flagged)
        [Required]
        public Activation Activation { get; set; } = Activation.active;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public List<Vote> Votes { get; set; } = new List<Vote>();

        // If a certain user has voted this post, this returns his vote. Else undefined.
        public Vote.Votes VotingByUser(String userName)
            => Votes.FirstOrDefault(v => v.UserId == userName)?.Upvote
            ?? Vote.Votes.Undefined;
    }
}
