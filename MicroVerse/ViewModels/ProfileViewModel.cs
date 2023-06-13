using MicroVerse.Models;

namespace MicroVerse.ViewModels
{
    public class ProfileViewModel
    {
        public ProfileViewModel(string username, string displayedName, string bio, int followerCount, int followsCount, List<PostViewModel> posts)
        {
            UserName = username;
            DisplayedName = displayedName;
            Bio = bio;
            FollowerCount = followerCount;
            FollowsCount = followsCount;
            Posts = posts;
        }



        public string UserName { get; set; }
        public string DisplayedName { get; set; }
        public string Bio { get; set; }
        public int FollowerCount { get; set; }
        public int FollowsCount { get; set; }
        public List<PostViewModel> Posts { get; set; }
        
    }
}
