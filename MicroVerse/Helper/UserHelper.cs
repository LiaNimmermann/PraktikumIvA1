using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MicroVerse.Data;
using MicroVerse.Models;
using MicroVerse.ViewModels;

namespace MicroVerse.Helper
{
    // The user helper manages database access for all things user related
    public class UserHelper
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User>? _userManager;

        public UserHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public UserHelper
        (
            ApplicationDbContext context,
            UserManager<User> userManager
        )
        {
        	_context = context;
          _userManager = userManager;
        }

        // Get a list with all users
        public IEnumerable<User> GetUsers() => _context.Users.ToList();

        // Get a specific user using his username
        public async Task<User?> GetUser(String userName)
            => await _context.Users
            .FirstOrDefaultAsync(user => userName == user.UserName);

        // Get a specific user using his userId
        public async Task<User?> GetUserId(String userId)
            => await _context.Users
            .FirstOrDefaultAsync(user => userId == user.Id);

        // Get a specific user object with all his follows
        public async Task<User?> GetUserWithFollows(String userName)
        {
            var fH = new FollowsHelper(_context);
            var user = await GetUser(userName);
            if (user is null)
            {
                return null;
            }
            var grouped = (await fH.GetFollows())
                .Where(f => f.FollowedUserId == user.UserName || f.FollowingUserId == user.UserName)
                .AsEnumerable()
                .GroupBy(f => f.FollowedUserId == user.UserName);

            user.Followers = grouped
                .FirstOrDefault(g => g.First().FollowedUserId == user.UserName)
                ?.Select(f => f.FollowingUserId) ?? Enumerable.Empty<String>();
            user.Following = grouped
                .FirstOrDefault(g => g.First().FollowingUserId == user.UserName)
                ?.Select(f => f.FollowedUserId) ?? Enumerable.Empty<String>();

            return user;
        }

        // Get all users with all their follows
        public async Task<IEnumerable<User>> GetUsersWithFollows()
        {
            var fH = new FollowsHelper(_context);
            var users = GetUsers();

            var follows = await fH.GetFollows();
            foreach (var user in users)
            {
                var grouped = follows
                    .Where(f => f.FollowedUserId == user.UserName || f.FollowingUserId == user.UserName)
                    .AsEnumerable()
                    .GroupBy(f => f.FollowedUserId == user.UserName);

                user.Followers = grouped
                    .FirstOrDefault(g => g.First().FollowedUserId == user.UserName)
                    ?.Select(f => f.FollowingUserId) ?? Enumerable.Empty<String>();
                user.Following = grouped
                    .FirstOrDefault(g => g.First().FollowingUserId == user.UserName)
                    ?.Select(f => f.FollowedUserId) ?? Enumerable.Empty<String>();
            }

            return users;
        }

        // Change an existing user in the database
        public async Task<Status> PutUser(String userName, User userModel)
        {
            if (userName != userModel.UserName)
                return Status.BadRequest;

            _context.Entry(userModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(userName))
                {
                    return Status.NotFound;
                }
                else
                {
                    throw;
                }
            }

            return Status.NoContent;
        }

        // Change an existing user in the database
        public async Task<User?> PostUser(User userModel)
        {
            _context.Users.Add(userModel);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserExists(userModel.UserName))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return userModel;
        }

        // Get a UserWithRole, i.e. a user object also including the user's role
        // (user, mod, admin)
        public IEnumerable<UserWithRole> GetUserWithRole()
            => GetUsers()
            .Select(u => new UserWithRole
                    (
                        u,
                        new IdentityRole
                        (
                            GetRoleOfUser(u).Result
                        )
                    )
            );

        // Delete a user
        public async Task<Status> DeleteUser(String userName)
            => await ChangeUser(userName, async user =>
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return Status.NoContent;
            });

        // Change the bio of a user
        public async Task<Status> ChangeBio(string userName, string bio)
            => await ChangeUser(userName, async user =>
            {
                user.Bio = bio;
                return await PutUser(userName, user);
            });

        // Change the display name of a user
        public async Task<Status> ChangeDisplayName(string userName, string displayName)
            => await ChangeUser(userName, async user =>
            {
                user.DisplayedName = displayName;
                return await PutUser(userName, user);
            });

        // Change the profile picture of a user
        public async Task<Status> ChangePicture(string userName, String imgLink)
            => await ChangeUser(userName, async user =>
            {
                user.Picture = imgLink;
                return await PutUser(userName, user);
            });

        // Block a user
        public async Task<Status> BlockUser(string id)
            => await ChangeUserActivation(id, Activation.blocked);

        // Ban a user
        public async Task<Status> BanUser(string id)
            => await ChangeUserActivation(id, Activation.banned);

        // Change the activation of a user (active, blocked, banned)
        private async Task<Status> ChangeUserActivation(String id, Activation activation)
            => await ChangeUser(id, async user =>
            {
                user.Activation = activation;
                return await PutUser(id, user);
            });

        // Get the role of a user
        public async Task<String> GetUserRole(string id)
        {
            var user = await GetUser(id)
                ?? throw new NullReferenceException($"User {id} does not exist.");

            return await GetRoleOfUser(user);
        }

        // Get the role of a user (helper method)
        private async Task<String> GetRoleOfUser(User user)
        {
            if (_userManager is null)
            {
                throw new NullReferenceException($"No UserManager available.");
            }

            if(await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return "Admin";
            }
            else if(await _userManager.IsInRoleAsync(user, "Moderator"))
            {
                return "Moderator";
            }
            else
            {
                return "User";
            }
        }

        // change a user
        private async Task<Status> ChangeUser
        (
            String id,
            Func<User, Task<Status>> change
        )
        {
            var user = await GetUser(id);
            if (user is null)
            {
                return Status.NotFound;
            }

            return await change(user);
        }

        // test if a user exists
        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.UserName == id);
        }
    }
}
