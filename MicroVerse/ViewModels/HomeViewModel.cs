using MicroVerse.Models;

namespace MicroVerse.ViewModels
{
    public class HomeViewModel
    {
        public List<User> Follows { get; set; }
        public List<PostViewModel> Posts { get; set; }


        public HomeViewModel(List<User> follows, List<PostViewModel> posts)
        {
            Follows = follows;
            Posts = posts;
        }

    }
}
