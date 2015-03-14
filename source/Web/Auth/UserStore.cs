using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using SiSystems.ClientApp.Web.Domain.Services;

namespace SiSystems.ClientApp.Web.Auth
{
    public class UserStore : IUserStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>
    {
        private readonly UserService _userService;
        private readonly ClientDetailsService _detailsService;

        public UserStore(UserService userService, ClientDetailsService detailsService)
        {
            _userService = userService;
            _detailsService = detailsService;
        }

        public Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return Task.Run(() =>
            {
                var user = _userService.FindByName(userName);
                if (user != null)
                {
                    var details = _detailsService.GetClientDetails(user.ClientId);
                    if (details != null)
                    {
                        return new ApplicationUser(user, details);
                    }
                }
                return null;
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

        public Task<ApplicationUser> FindByIdAsync(string userId)
        {
            return Task.Run(() =>
            {
                int id;
                int.TryParse(userId, out id);

                var user = _userService.Find(id);
                if (user != null)
                {
                    var details = _detailsService.GetClientDetails(user.ClientId);
                    if (details != null)
                    {
                        return new ApplicationUser(user, details);
                    }
                }
                return null;
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
