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
        	return _context.Post.ToList();
        }
        
        public Post GetPost(Guid id)
        {
        	return _context.Post
        		.ToList()
        		.FirstOrDefault(p => p.Id == id);
        }
    }
}
