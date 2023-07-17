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

        public string displayedName { get; set; }


        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;


        
        public string Picture  { get; set; } 

    }
}
