using MicroVerse.Data;
using MicroVerse.Models;

namespace MicroVerse.Helper
{
    public class SearchHelper
    {
    	private readonly ApplicationDbContext _context;

        public SearchHelper(ApplicationDbContext context)
        {
        	_context =context;
        }
        
        public IEnumerable<Post> Posts(String phrase)
        {
        	return _context.Post
                .AsEnumerable()
                .Where(p => p.FuzzyMatches(phrase));
        }
        
        public IEnumerable<User> Users(String phrase)
        {
        	return _context.Users
                .AsEnumerable()
                .Where(u => u.FuzzyMatches(phrase));
        }
    }
}
