using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroVerseMaui.Models
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
        
        public string AuthorImage  { get; set; } = "https://picsum.photos/200/200";

    }
}
