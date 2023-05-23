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
            Username = username;
            DisplayedName = displayedName;
            Bio = bio;
            CreatedAt = DateTime.Now;
        }

        [Required]
        [Key]
        [EmailAddress]
        public String Email { get; set; } = "";

        [Required]
        public String Username { get; set; } = "";

        [Required]
        public String DisplayedName { get; set; } = "";

        [Required]
        public Role Role { get; set; } = Role.user;

        public String Picture { get; set; } = "";

        public String Bio { get; set; } = "";

        [Required]
        public Activation Activation { get; set; } = Activation.active;

        public DateTime CreatedAt { get; } = DateTime.Now;

        public List<Follows> Follows { get; set; } = new List<Follows>();

        [Required]
        [DataType(DataType.Password)]
        public String Password { get; set; } = "";

        public void Delete() {
            throw new NotImplementedException();
        }
    }
}
