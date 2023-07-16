using MicroVerse.Models;
using MicroVerse.ViewModels;

namespace MicroVerse.Helper
{
    public static class UsefulExtensions
    {
        // Converts a list of posts to a list of PostViewModels
        internal static List<PostViewModel> PostsToViewModel
            (
                this IEnumerable<Post> postsList,
                IEnumerable<User> users,
                string currentUser
            )
            => postsList.Select(post => new PostViewModel(post, users)
            {
                VoteByUser = post.VotingByUser(currentUser)
            }).ToList();

    }
}
