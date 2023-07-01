using MicroVerse.Models;

namespace MicroVerse.ViewModels
{
    public class ProfileViewModel
    {
        public ProfileViewModel
        (
            string username,
            string displayedName,
            string bio,
            IEnumerable<User> followers,
            IEnumerable<User> following,
            List<PostViewModel> posts,
            bool follows,
            string role
        )
        {
            UserName = username;
            DisplayedName = displayedName;
            Bio = bio;
            FollowerCount = followers.Count();
            FollowsCount = following.Count();
            Posts = posts;
            Follows = new FollowButtonModel(username, follows);
            Role = role;
        }



        public string UserName { get; set; }
        public string DisplayedName { get; set; }
        public string Bio { get; set; }
        public int FollowerCount { get; set; }
        public int FollowsCount { get; set; }
        public List<PostViewModel> Posts { get; set; }
        public FollowButtonModel Follows { get; set; }
        public string Role { get; set; }
    }
}
