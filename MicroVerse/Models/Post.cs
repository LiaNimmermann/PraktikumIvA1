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
        public Post(string body, string authorId, Post reactsTo)
        {
            Id = Guid.NewGuid();
            Body = body;
            AuthorId = authorId;
            ReactsTo = reactsTo;
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
    }
}
