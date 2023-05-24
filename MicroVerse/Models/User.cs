using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Transactions;

namespace MicroVerse.Models
{
    public class User : IdentityUser
    {
        public User()
        {
        }

        public User(string email, string userName,string displayedName, Role role)
        {
            Email = email;
            UserName = userName;
            DisplayedName = displayedName;
            Role = role;
        }

        [Required]
        public string DisplayedName { get; set; } = "";

        [Required]
        public Role Role { get; set; } = Role.user;

        public byte[]? Picture { get; set; }

        public string Bio { get; set; } = "Hi, I'm new here!";

        //[Required]
       // public Activation Activation { get; set; } = Activation.active;
    }
}
