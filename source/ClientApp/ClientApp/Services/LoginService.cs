using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ClientApp.Services.Interfaces;
using SiSystems.ClientApp.SharedModels;

namespace ClientApp.Services
{
    public class LoginService : ILoginService
    {
        public Task<ValidationResult> LoginAsync(string username, string password)
        {
            //TODO connect to webservice and make call
            return Task.FromResult(new ValidationResult(true));
        }
    }
}
