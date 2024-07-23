using ecommapp.Data;
using ecommapp.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ecommapp.Services
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<AuthenticationResult> AuthenticateAsync(string userName, string password)
        {
            var result = new AuthenticationResult();

            var userInfo = await _context.Users
                .FirstOrDefaultAsync(x => x.Username == userName && x.Password == password);

            if (userInfo == null)
            {
                return result;
            }

            result.IsSuccess = true;
            result.Roles.Add(userInfo.Role);

            return result;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            if(await _context.Users.AnyAsync(u => u.Username == user.Username))
            {
                throw new InvalidOperationException("A user with this username already exists.");
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }



    public class AuthenticationResult
    {
        public bool IsSuccess { get; set; } = false;
        public List<string> Roles { get; set; } = new List<string>();
    }

    public class Roles
    {
        public const string Admin = "Admin";
        public const string Regular = "Regular";
    }
}
