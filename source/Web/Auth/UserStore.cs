using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using SiSystems.ClientApp.Web.Domain;

namespace SiSystems.ClientApp.Web.Auth
{
    public class UserStore : IUserStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>
    {
        private readonly UserService _userService;

        public UserStore(UserService userService)
        {
            _userService = userService;
        }

        public Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return Task.Run(() =>
            {
                var user = _userService.FindByName(userName);
                return user != null
                    ? new ApplicationUser(user)
                    : null;
            });
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user)
        {
            return Task.Run(() =>
            {
                var existingUser = _userService.FindByName(user.UserName);
                return existingUser != null ? existingUser.PasswordHash : null;
            });
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user)
        {
            return Task.Run(() =>
            {
                var existingUser = _userService.FindByName(user.UserName);
                return existingUser != null && !string.IsNullOrEmpty(existingUser.PasswordHash);
            });
        }

        public void Dispose()
        {

        }

        #region Not Implemented
        public Task CreateAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> FindByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }
        public Task UpdateAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
