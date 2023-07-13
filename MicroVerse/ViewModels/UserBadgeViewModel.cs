using MicroVerse.Models;

namespace MicroVerse.ViewModels
{
    public class UserBadgeViewModel
    {
        public UserBadgeViewModel
        (
	 	User user,
        	bool follows
        )
        {
            UserName = user.UserName;
            DisplayedName = user.DisplayedName;
            Follows = new FollowButtonModel(user.UserName, follows);
	    Picture = user.Picture;
        }



        public string UserName { get; set; }
        public string DisplayedName { get; set; }
        public FollowButtonModel Follows { get; set; }
	public String Picture { get; set; }
        
    }
}
