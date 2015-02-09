using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using SiSystems.ClientApp.SharedModels;

namespace SiSystems.ClientApp.Web.Auth
{
    public class ApplicationUser : IUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            
            // Add custom user claims here
            //TODO: Custom Claims?

            return userIdentity;
        }

        private readonly User _user;

        public string Id
        {
            get { return _user.Id.ToString(); }
        }

        public string UserName
        {
            get { return _user.Login; }
            set { _user.Login = value; }
        }

        public ApplicationUser(User user)
        {
            _user = user;
        }
    }
}
