using MicroVerse.Models;
using System.Text;

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
            string role,
            byte[] pictureHash
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
            if( pictureHash != null )
            {
                PictureUrl = "https://picsum.photos/id/" + Encoding.UTF8.GetString(pictureHash) + "/200/200"; ;
            }
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
