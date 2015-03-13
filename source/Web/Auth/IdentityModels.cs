using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using SiSystems.ClientApp.SharedModels;
using System.Linq;

namespace SiSystems.ClientApp.Web.Auth
{
    public class ApplicationUser : IUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

            // Add custom user claims here

            return userIdentity;
        }

        private static MatchGuideConstants.FloThruAlumniAccess[] allowedAccessLevels = new MatchGuideConstants.FloThruAlumniAccess[] {
                MatchGuideConstants.FloThruAlumniAccess.AllAccess,
                MatchGuideConstants.FloThruAlumniAccess.LimitedAccess
            };

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

        public bool UserHasAccess
        {
            get { return allowedAccessLevels.Contains(_user.FloThruAlumniAccess); }
        }

        public bool CompanyHasAccess
        {
            get { return _details.HasAccess; }
        }

        public bool IsGrantedAccess
        {
            get
            {
                return UserHasAccess && CompanyHasAccess;
            }
        }

        public ApplicationUser(User user, ClientAccountDetails details)
        {
            _user = user;
            _details = details;
        }
    }
}
