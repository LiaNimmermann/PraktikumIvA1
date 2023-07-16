using Microsoft.EntityFrameworkCore;
using MicroVerse.Data;
using MicroVerse.Models;

namespace MicroVerse.Helper
{
    // The follows helper manages database access for all things follows related
    public class FollowsHelper
    {
        private readonly ApplicationDbContext _context;

        public FollowsHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        // get all follows relationships
        public async Task<IEnumerable<Follows>> GetFollows()
            => await _context.Follows.ToListAsync();

        // get all followers of a certain user
        public async Task<IEnumerable<User>> GetFollowers(String userName)
            => await _context.Follows
            .Where(f => f.FollowedUserId == userName)
            .Join
            (
                _context.Users,
                follow => follow.FollowingUserId,
                user => user.UserName,
                (follow, user) => user
            )
            .ToListAsync();

        // get all users that a certain user follows
        public async Task<IEnumerable<User>> GetFollowings(String userName)
            => await _context.Follows
            .Where(f => f.FollowingUserId == userName)
            .Join
            (
                _context.Users,
                    follow => follow.FollowedUserId,
                    user => user.UserName,
                    (follow, user) => user
                    )
            .ToListAsync();

        // true if following follows followed
        public async Task<Boolean> Follows(String following, String followed)
            => (await GetFollowers(followed))
            .Any(u => u.UserName == following);

        // follow a user
        public async Task FollowUser(String follower, String followed)
        {
            var follows = new Follows(follower, followed);
            _context.Follows.Add(follows);
            await _context.SaveChangesAsync();
        }

        // unfollow a user
        public async Task UnfollowUser(string followerId, string followedId)
        {
            var toDelete = _context.Follows.First(f => f.FollowingUserId == followerId && f.FollowedUserId == followedId);
            _context.Follows.Remove(toDelete);
            await _context.SaveChangesAsync();
        }
    }
}
