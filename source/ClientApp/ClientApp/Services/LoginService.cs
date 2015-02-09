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

        public Task<ValidationResult> LoginAsync(string username, string password)
        {
            return _connection.Login(username, password);
        }

        public OAuthToken GetAuthToken()
        {
            return _connection.Token;
        }

        public void SetAuthToken(OAuthToken token)
        {
            _connection.Token = token;
        }
    }
}
