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
            userIdentity.AddClaim(new Claim(CustomClaimTypes.FloThruAlumniAccessLevel, this._user.FloThruAlumniAccess.ToString()));
            userIdentity.AddClaim(new Claim(CustomClaimTypes.CompanyAccess, this._details.HasAccess.ToString()));
            userIdentity.AddClaim(new Claim(CustomClaimTypes.Company, this._user.CompanyName));

            return userIdentity;
        }

        private readonly User _user;
        private readonly ClientAccountDetails _details;

        public string Id
        {
            get { return _user.Id.ToString(); }
        }

        public string UserName
        {
            get { return _user.Login; }
            set { _user.Login = value; }
        }

        public ApplicationUser(User user, ClientAccountDetails details)
        {
            _user = user;
            _details = details;
        }
    }
}
