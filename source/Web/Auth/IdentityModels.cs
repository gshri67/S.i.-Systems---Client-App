using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Linq;
using SiSystems.SharedModels;

namespace SiSystems.ClientApp.Web.Auth
{
    public class ApplicationUser : IUser
    {
        private static MatchGuideConstants.FloThruAlumniAccess[] allowedAccessLevels = new MatchGuideConstants.FloThruAlumniAccess[] {
                MatchGuideConstants.FloThruAlumniAccess.AllAccess,
                MatchGuideConstants.FloThruAlumniAccess.LimitedAccess
            };

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

            // Add custom user claims here

            return userIdentity;
        }

        private readonly User _user;

        public ApplicationUser(User user)
        {
            _user = user;
        }

        public string Id
        {
            get { return _user.Id.ToString(); }
        }

        public string UserName
        {
            get { return _user.Login; }
            set { _user.Login = value; }
        }

        public int CompanyId { get { return _user.CompanyId; } }

        public string CompanyName
        {
            get { return _user.CompanyName; }
        }

        public MatchGuideConstants.FloThruAlumniAccess AccessLevel
        {
            get { return _user.FloThruAlumniAccess; }
        }

        public bool IsCompanyParticipating
        {
            get { return _user.IsCompanyParticipating; }
        }

        public bool IsGrantedAccess
        {
            get {
                return _user.IsCompanyParticipating
                  && allowedAccessLevels.Contains(this.AccessLevel);
            }
        }
    }
}
