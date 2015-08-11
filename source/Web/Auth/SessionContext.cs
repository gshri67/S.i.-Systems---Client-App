using System.Web;
using Microsoft.AspNet.Identity;
using SiSystems.Web.Domain.Context;
using SiSystems.ClientApp.Web.Domain.Services;
using System.Web.Http;
using System.Net;
using System.Threading.Tasks;
using SiSystems.SharedModels;

namespace SiSystems.ClientApp.Web.Auth
{
    public class SessionContext : ISessionContext
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
            // This ocurrs when we switch from Debug -> Dev because the user has a
            // valid token, but the user does not exist in the database
            if (user == null)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }
            return user;
        }
    }
}
