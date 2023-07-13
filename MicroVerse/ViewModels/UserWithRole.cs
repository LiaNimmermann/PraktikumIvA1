using Microsoft.AspNetCore.Identity;
using MicroVerse.Models;

namespace MicroVerse.ViewModels
{
    public class UserWithRole
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string DisplayedName { get; set; }

        public String? Picture { get; set; }

        public string Bio { get; set; }

        public Activation Activation { get; set; }
        public IdentityRole Role { get; set; }


        public UserWithRole(User user, IdentityRole role)
        {
            UserName = user.UserName;
            DisplayedName = user.DisplayedName;
            Email = user.Email;
            Picture = user.Picture;
            Bio = user.Bio;
            Activation = user.Activation;
            Role = role;
        }
    }
}
