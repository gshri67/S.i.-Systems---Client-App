using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SiSystems.ClientApp.SharedModels;

namespace ClientApp.Core.ViewModels
{

    public class LoginViewModel : ViewModelBase
    {
        private readonly IMatchGuideApi _api;
        public Dictionary<string, int> EulaVersions { get; private set; }
        public string UserName { get; set; }

        public LoginViewModel(IMatchGuideApi api)
        {
            this._api = api;
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
            var result = await this._api.Login(username, password);
            if (result.IsValid)
            {
                UserName = username;
            }
            return result;
        }

        public async Task<Eula> GetCurrentEulaAsync()
        {
            return await _api.GetMostRecentEula();
        }

        public bool UserHasReadLatestEula(string username, int version, string storageString)
        {
            try
            {
                EulaVersions = JsonConvert.DeserializeObject<Dictionary<string, int>>(storageString);
            }
            catch (Exception)
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

        public Task<ClientAccountDetails> GetClientDetailsAsync()
        {
            return _api.GetClientDetails();
        }
    }
}
