using MicroVerse.Models;

namespace MicroVerse.ViewModels
{
    // a view model for the profile view
    public class ProfileViewModel
    {
        public ProfileViewModel
        (
            User user,
            List<PostViewModel> posts,
            bool follows,
            string role
        )
        {
            UserName = user.UserName;
            DisplayedName = user.DisplayedName;
            Bio = user.Bio;
            FollowerCount = user.Followers.Count();
            FollowsCount = user.Following.Count();
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
