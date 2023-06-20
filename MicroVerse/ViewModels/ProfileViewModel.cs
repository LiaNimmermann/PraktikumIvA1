using MicroVerse.Models;

namespace MicroVerse.ViewModels
{
    public class ProfileViewModel
    {
        public ProfileViewModel(string username, string displayedName, string bio, int followerCount, int followsCount, List<PostViewModel> posts, bool follows)
        {
            UserName = username;
            DisplayedName = displayedName;
            Bio = bio;
            FollowerCount = followerCount;
            FollowsCount = followsCount;
            Posts = posts;
            Follows= new FollowButtonModel(username, follows);
        }



        public string UserName { get; set; }
        public string DisplayedName { get; set; }
        public string Bio { get; set; }
        public int FollowerCount { get; set; }
        public int FollowsCount { get; set; }
        public List<PostViewModel> Posts { get; set; } 
        public FollowButtonModel Follows { get; set; }
        
    }
}
