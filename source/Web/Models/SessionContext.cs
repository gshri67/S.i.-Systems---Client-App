using SiSystems.ClientApp.SharedModels;
using SiSystems.ClientApp.Web.Domain;

namespace SiSystems.ClientApp.Web.Models
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
            //TODO: GET THE ACTUAL USER ONCE AUTHENTICATED..
            return _userService.FindByName("bob.smith@email.com");
        }
    }
}
