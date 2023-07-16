using MicroVerse.Data;
using MicroVerse.Models;

namespace MicroVerse.Helper
{
    // A helper for search actions
    public class SearchHelper
    {
    	private readonly ApplicationDbContext _context;

        public SearchHelper(ApplicationDbContext context)
        {
        	_context = context;
        }

        // search all users which contain the string phrase in their data
        public IEnumerable<User> Users(String phrase)
        {
        	return _context.Users
                .AsEnumerable()
                .Where(u => u.FuzzyMatches(phrase));
        }
    }
}
