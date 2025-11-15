using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace MarminaAttendance.Identity
{
    public class CustomUserManager : UserManager<ApplicationUser>
    {
        public CustomUserManager(
            IUserStore<ApplicationUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IEnumerable<IUserValidator<ApplicationUser>> userValidators,
            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<ApplicationUser>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators,
                   passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        // Get user by ID with Servant
        public async Task<ApplicationUser> FindByIdWithServantAsync(string userId)
        {
            return await Users
                .Include(u => u.Servant)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        // Get user by Email with Servant
        public async Task<ApplicationUser> FindByUserNameWithServantAsync(string userName)
        {
            return await Users
                .Include(u => u.Servant)
                .FirstOrDefaultAsync(u => u.UserName == userName);
        }

        // Get user by Username with Servant
        public async Task<ApplicationUser> GetUserWithServantAsync(ClaimsPrincipal User)
        {
            return await Users
                .Include(u => u.Servant)
                .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
        }

        // Get all users with Servants
        public async Task<List<ApplicationUser>> GetAllUsersWithServantsAsync()
        {
            return await Users
                .Include(u => u.Servant)
                .ToListAsync();
        }
    }
}