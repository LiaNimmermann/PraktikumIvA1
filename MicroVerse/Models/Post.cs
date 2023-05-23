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
        public int Id { get; set; }
        public string Body { get; set; }    
        public UserModel Author { get; set; }
        public Post? ReactsTo { get; set; }
        public Activation Activation { get; set; } = Activation.active;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public List<Vote> Votes { get; set; } = new List<Vote>();
    }
}
