using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroVerse.Models
{
    // the user data class
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
            //Role = role;
        }

        [Required]
        public string DisplayedName { get; set; } = "";

        //[Required]
        //public Role Role { get; set; } = Role.user;

        public String? Picture { get; set; }

        public string Bio { get; set; } = "Hi, I'm new here!";

        // the activation of the user (active, blocked, banned)
        [Required]
        public Activation Activation { get; set; } = Activation.active;

        // the usernames of the followers of the user
        [NotMapped]
        public IEnumerable<String> Followers { get; set; } = new List<String>();

        // the usernames of the users that this user follows
        [NotMapped]
        public IEnumerable<String> Following { get; set; } = new List<String>();

        // match phrase against all data of this user
        public Boolean FuzzyMatches(String phrase)
        {
        	var lcPhrase = phrase.ToLower();
        	return DisplayedName.ToLower().Contains(lcPhrase)
            || Bio.ToLower().Contains(lcPhrase)
            || UserName.ToLower().Contains(lcPhrase)
            || Email.ToLower().Contains(lcPhrase);
        }
    }
}
