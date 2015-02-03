using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientApp.Services.Interfaces;
using SiSystems.ClientApp.SharedModels;

namespace ClientApp.ViewModels
{

    public class LoginViewModel : ViewModelBase
    {
        private readonly ILoginService _loginService;
        private readonly IEulaService _eulaService;

        public LoginViewModel(ILoginService loginService, IEulaService eulaService)
        {
            _loginService = loginService;
            _eulaService = eulaService;
        }

        public ValidationResult IsValidUserName(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return new ValidationResult(false, "Please enter a user name");
            }
            return new ValidationResult(true);
        }

        public ValidationResult IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return new ValidationResult(false, "Please enter a password");
            }
            //TODO find out any password rules we have to constrain them to
            if (password.Length < 3)
            {
                return new ValidationResult(false, "Password must be at least 4 characters");
            }
            return new ValidationResult(true);
        }

        public Task<ValidationResult> LoginAsync(string username, string password)
        {
            return _loginService.LoginAsync(username, password);
        }

        public Task<Eula> GetCurrentEulaAsync()
        {
            return _eulaService.GetMostRecentEula();
        }
    }
}
