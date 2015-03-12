using System.Web;
using Microsoft.AspNet.Identity;
using SiSystems.ClientApp.SharedModels;
using SiSystems.ClientApp.Web.Domain;
using SiSystems.ClientApp.Web.Domain.Context;
using SiSystems.ClientApp.Web.Domain.Services;
using System.Web.Http;
using System.Net;

namespace SiSystems.ClientApp.Web.Auth
{
    public class SessionContext: ISessionContext
    {
        private User _currentUser;
        public User CurrentUser
        {
            get { return _currentUser ?? (_currentUser = GetCurrentAuthenticatedUser()); }
        }
        
        private readonly UserService _userService;
        public SessionContext(UserService service)
        {
            _userService = service;
        }

        private User GetCurrentAuthenticatedUser()
        {
            var userId = int.Parse(HttpContext.Current.User.Identity.GetUserId());
            var user = _userService.Find(userId);
            if (user == null)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }
            return user;
        }
    }
}
