using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MicroVerse.Data;
using MicroVerse.Models;
using MicroVerse.ViewModels;

namespace MicroVerse.Helper
{
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

        public async Task<IEnumerable<User>> GetUsers()
        {
            var fH = new FollowsHelper(_context);
            var users = _context.Users.ToList();
            foreach (var user in users)
            {
                user.Followers = (await fH.GetFollowers(user.UserName))
                    .Select(u => u.UserName);
                user.Following = (await fH.GetFollowings(user.UserName))
                    .Select(u => u.UserName);
            }
            return users;
        }

        public async Task<User?> GetUser(String userName)
            => await _context.Users
            .FirstOrDefaultAsync(user => userName == user.UserName);


        public async Task<User?> GetUserId(String userId)
            => await _context.Users
            .FirstOrDefaultAsync(user => userId == user.Id);

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

        public async Task<IEnumerable<UserWithRole>> GetUserWithRole()
        {
            var users = await GetUsers();

            return users.Select(u => new UserWithRole
                                (
                                    u,
                                    new IdentityRole
                                    (
                                        GetRoleOfUser(u).Result
                                    )
                                )
            );
        }

        public async Task<Status> DeleteUser(String userName)
            => await ChangeUser(userName, async user =>
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return Status.NoContent;
            });

        public async Task<Status> BlockUser(string id)
            => await ChangeUserActivation(id, Activation.blocked);

        public async Task<Status> BanUser(string id)
            => await ChangeUserActivation(id, Activation.banned);

        private async Task<Status> ChangeUserActivation(String id, Activation activation)
            => await ChangeUser(id, async user =>
            {
                user.Activation = activation;
                return await PutUser(id, user);
            });

        public async Task<String> GetUserRole(string id)
        {
            var user = await GetUser(id)
                ?? throw new NullReferenceException($"User {id} does not exist.");

            return await GetRoleOfUser(user);
        }

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

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.UserName == id);
        }
    }
}
