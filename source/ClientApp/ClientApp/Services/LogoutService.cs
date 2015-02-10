using System;
using System.Collections.Generic;
using System.Text;
using ClientApp.Services.Interfaces;

namespace ClientApp.Services
{
    public class LogoutService : ILogoutService
    {
        private readonly IConnectionService _connection;

        public LogoutService(IConnectionService connection)
        {
            _connection = connection;
        }

        public void Logout()
        {
            _connection.Post("logout", null);
            _connection.Token = null;
        }
    }
}
