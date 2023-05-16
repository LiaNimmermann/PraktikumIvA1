using Microsoft.Extensions.Hosting;
using MicroVerse.Models;

namespace MicroVerse.ViewModels
{
    public class HomeViewModel
    {
        //User that is currently logged in
        public UserModel User { get; set; }
        public List<UserModel> Follows { get; set; }
        public List<PostViewModel> Posts { get; set; }


        public HomeViewModel(UserModel user, List<UserModel> follows, List<PostViewModel> posts)
        {
            this.User = user;
            this.Follows = follows;
            this.Posts = posts;
        }

    }
}
