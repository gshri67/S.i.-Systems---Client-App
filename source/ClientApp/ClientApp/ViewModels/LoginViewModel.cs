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
        public string UserNameError { get; set; }
        public string Password { get; set; }
        public string PasswordError { get; set; }

        private readonly ILoginService _loginService;

        public LoginViewModel(ILoginService loginService)
        {
            _loginService = loginService;
        }

        public Boolean IsValidUserName()
        {
            if (string.IsNullOrEmpty(UserName))
            {
                UserNameError = "Please enter a user name";
                return false;
            }
            return true;
        }

        public bool IsValidPassword()
        {
            if (string.IsNullOrEmpty(Password))
            {
                PasswordError = "Please enter a password";
                return false;
            }
            //TODO find out any password rules we have to constrain them to
            if (Password.Length < 3)
            {
                PasswordError = "Password must be at least 4 characters";
                return false;
            }
            return true;
        }

        public Task<bool> LoginAsync()
        {
            return _loginService.LoginAsync(UserName, Password);
        }
    }
}
