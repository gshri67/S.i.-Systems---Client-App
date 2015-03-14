using System.Web;
using Microsoft.AspNet.Identity;
using SiSystems.ClientApp.SharedModels;
using SiSystems.ClientApp.Web.Domain.Context;
using SiSystems.ClientApp.Web.Domain.Services;
using System.Web.Http;
using System.Net;

namespace SiSystems.ClientApp.Web.Auth
{
    public class SessionContext: ISessionContext
    {
        private User _currentUser;
        private ClientAccountDetails _details;

        public User CurrentUser
        {
            get { return _currentUser ?? (_currentUser = GetCurrentAuthenticatedUser()); }
        }

        public ClientAccountDetails CurrentUserDetails
        {
            get { return _details ?? (_details = GetCurrentAuthenticatedUserDetails()); }
        }

        public bool IsAuthorized
        {
            get
            {
                var applicationUser = new ApplicationUser(this.CurrentUser, this.CurrentUserDetails);
                return applicationUser.IsGrantedAccess;
            }
        }
        
        private readonly UserService _userService;
        private readonly ClientDetailsService _detailsService;

        public SessionContext(UserService service, ClientDetailsService detailsService)
        {
            _userService = service;
            _detailsService = detailsService;
        }

        private User GetCurrentAuthenticatedUser()
        {
            var userId = int.Parse(HttpContext.Current.User.Identity.GetUserId());
            var user = _userService.Find(userId);
            // This ocurrs when we switch from Debug -> Dev because the user has a
            // valid token, but the user does not exist in the database
            if (user == null)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }
            return user;
        }

        private ClientAccountDetails GetCurrentAuthenticatedUserDetails()
        {
            var user = this.GetCurrentAuthenticatedUser();
            var details = _detailsService.GetClientDetails(user.ClientId);
            return details;
        }
    }
}
