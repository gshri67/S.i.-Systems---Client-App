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
        private readonly IConnectionService _connection;

        public LoginService(IConnectionService connection)
        {
            _connection = connection;
        }

        public async Task<ValidationResult> LoginAsync(string username, string password)
        {
            var result = await _connection.Login(username, password);
            return result
                ? new ValidationResult(true)
                : new ValidationResult(false, "The username or password is incorrect");
        }
    }
}
