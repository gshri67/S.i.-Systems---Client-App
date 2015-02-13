using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientApp.Services.Interfaces;
using Newtonsoft.Json;
using SiSystems.ClientApp.SharedModels;

namespace ClientApp.ViewModels
{

    public class LoginViewModel : ViewModelBase
    {
        private readonly ILoginService _loginService;
        private readonly IEulaService _eulaService;
        public Dictionary<string, int> EulaVersions { get; private set; }
        public string UserName { get; set; }

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
            if (password.Length < 4)
            {
                return new ValidationResult(false, "Password must be at least 4 characters");
            }
            return new ValidationResult(true);
        }

        public async Task<ValidationResult> LoginAsync(string username, string password)
        {
            var result = await _loginService.LoginAsync(username, password);
            if (result.IsValid)
            {
                UserName = username;
            }
            return result;
        }

        public Task<Eula> GetCurrentEulaAsync()
        {
            return _eulaService.GetMostRecentEula();
        }

        public bool UserHasReadLatestEula(string username, int version, string storageString)
        {
            try
            {
                EulaVersions = JsonConvert.DeserializeObject<Dictionary<string, int>>(storageString);
            }
            catch (Exception ex)
            {
                //TODO log error
                EulaVersions = new Dictionary<string, int>();
                return false;
            }

            if (EulaVersions.ContainsKey(username))
            {
                if (EulaVersions[username] == version)
                {
                    return true;
                }
            }
            return false;
        }

        public void SetAuthToken(OAuthToken token)
        {
            UserName = token.Username;
            _loginService.SetAuthToken(token);
        }

        public OAuthToken GetAuthToken()
        {
            return _loginService.GetAuthToken();
        }
    }
}
