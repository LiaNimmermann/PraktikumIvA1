using MicroVerse.Models;
using System.Text;

namespace MicroVerse.ViewModels
{
    public class ProfileViewModel
    {
        public ProfileViewModel
        (
            User user,
            IEnumerable<User> followers,
            IEnumerable<User> following,
            List<PostViewModel> posts,
            bool follows,
            string role
        )
        {
            UserName = user.UserName;
            DisplayedName = user.DisplayedName;
            Bio = user.Bio;
            FollowerCount = followers.Count();
            FollowsCount = following.Count();
            Posts = posts;
            Follows = new FollowButtonModel(user.UserName, follows);
            Role = role;
            PictureUrl = user?.Picture ?? "";
        }



        public string UserName { get; set; }
        public string DisplayedName { get; set; }
        public string Bio { get; set; }
        public int FollowerCount { get; set; }
        public int FollowsCount { get; set; }
        public List<PostViewModel> Posts { get; set; }
        public FollowButtonModel Follows { get; set; }
        public string Role { get; set; }
        public string PictureUrl { get; set; } = "https://picsum.photos/200/200";
    }
}
