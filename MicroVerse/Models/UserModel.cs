using System.ComponentModel.DataAnnotations;

namespace MicroVerse.Models
{
    public class UserModel
    {
        public UserModel()
        {
        }

        //Just for mock data
        public UserModel(String username, string displayedName, string bio)
        {
            this.Username = new Username(username);
            DisplayedName = displayedName;
            Bio = bio;
            CreatedAt = DateTime.Now;
        }

        [Required]
        [EmailAddress]
        public String Email { get; set; } = "";

        [Required]
        public Username Username { get; set; } = new Username();

        [Required]
        public String DisplayedName { get; set; } = "";

        [Required]
        public Role Role { get; set; }

        public String Picture { get; set; } = "";

        public String Bio { get; set; } = "";

        [Required]
        public Activation Activation { get; set; }

        public DateTime CreatedAt { get; } = DateTime.Now;

        [Required]
        [DataType(DataType.Password)]
        public String Password { get; set; } = "";

        public void Delete() {
            throw new NotImplementedException();
        }
    }
}
