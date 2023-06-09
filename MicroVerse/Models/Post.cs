using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace MicroVerse.Models
{
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

        [Required]
        public Activation Activation { get; set; } = Activation.active;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public List<Vote> Votes { get; set; } = new List<Vote>();

        public Boolean FuzzyMatches(String phrase)
            => AuthorId.Contains(phrase)
            || Body.Contains(phrase)
            || (ReactsTo?.Body.Contains(phrase) ?? false);
    }
}
