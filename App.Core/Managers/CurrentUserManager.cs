using App.Core.Entities;
using MarminaAttendance.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace App.Core.Managers
{
    public class CurrentUserManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CustomUserManager _userManager;

        public CurrentUserManager(IHttpContextAccessor httpContextAccessor, CustomUserManager userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<Servants?> GetCurrentServantAsync()
        {
            var userName = _httpContextAccessor.HttpContext?.User?.Identity.Name;
            if (string.IsNullOrEmpty(userName))
                return null;

            var user = await _userManager.FindByUserNameWithServantAsync(userName);
            return user?.Servant;
        }
    }
}
