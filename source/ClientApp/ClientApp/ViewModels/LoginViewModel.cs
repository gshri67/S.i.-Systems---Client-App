using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientApp.Services.Interfaces;

namespace ClientApp.ViewModels
{

    public class LoginViewModel : ViewModelBase
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        private readonly ILoginService _loginService;

        public LoginViewModel(ILoginService loginService)
        {
            _loginService = loginService;
        }

        //TODO Terrible way of doing this, find something better
        public string GetUserNameError()
        {
            if (string.IsNullOrEmpty(UserName))
            {
                return "Please enter a user name";
            }
            return null;
        }

        public string GetPasswordError()
        {
            if (string.IsNullOrEmpty(Password))
            {
                return "Please enter a password";
            }
            //TODO find out any password rules we have to constrain them to
            if (Password.Length < 3)
            {
                return "Password must be at least 4 characters";
            }
            return null;
        }

        public Task<bool> LoginAsync()
        {
            return _loginService.LoginAsync(UserName, Password);
        }
    }
}
