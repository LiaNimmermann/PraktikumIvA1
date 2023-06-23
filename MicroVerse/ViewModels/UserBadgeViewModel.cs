using MicroVerse.Models;

namespace MicroVerse.ViewModels
{
    public class UserBadgeViewModel
    {
        public UserBadgeViewModel
        (
        	string username,
        	string displayedName,
        	bool follows
        )
        {
            UserName = username;
            DisplayedName = displayedName;
            Follows= new FollowButtonModel(username, follows);
        }



        public string UserName { get; set; }
        public string DisplayedName { get; set; }
        public FollowButtonModel Follows { get; set; }
        
    }
}
