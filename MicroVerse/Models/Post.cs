using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace MicroVerse.Models
{
    //not finished
    public class Post
    {
        public Post()
        {
        }

        public Post(string body, UserModel author)
        {
            Body = body;
            Author = author;

        }

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Body { get; set; }    

        [Required]
        public UserModel Author { get; set; }

        public Post? ReactsTo { get; set; }

        [Required]
        public Activation Activation { get; set; } = Activation.active;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public List<Vote> Votes { get; set; } = new List<Vote>();
    }
}
