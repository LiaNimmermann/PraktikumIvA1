using MicroVerse.Data;
using MicroVerse.Models;

namespace MicroVerse.Helper
{
    public class PostHelper
    {
    	private readonly ApplicationDbContext _context;

        public PostHelper(ApplicationDbContext context)
        {
        	_context =context;
        }
        
        public IEnumerable<Post> GetPosts()
        {
        	return _context.Post
        		.OrderByDescending(post => post.CreatedAt)
        		.ToList();
        }
        
        public IEnumerable<Post> GetPostsByUser(String userName)
        {
        	return GetPosts()
        		.Where(p => p.AuthorId == userName);
        }
        
        public IEnumerable<Post> GetPostsByUserAndFollows(String userName)
        {
        	
            var followsList = _context.Follows
            	.Where(f => f.FollowingUserId == userName)
            	.Select(f => f.FollowedUserId)
            	.ToList();
            followsList.Add(userName);
            var posts = followsList
            	.SelectMany(f => GetPostsByUser(f))
        		.OrderByDescending(post => post.CreatedAt);
            return posts;
        }
        
        public Post GetPost(Guid id)
        {
        	return GetPosts()
        		.FirstOrDefault(p => p.Id == id);
        }
    }
}
