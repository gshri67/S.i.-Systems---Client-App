using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ClientApp.Services.Interfaces;
using SiSystems.ClientApp.SharedModels;

namespace ClientApp.Services
{
    public class EmailService : IEmailService
    {
        private IConnectionService _connection;

        public EmailService(IConnectionService connection)
        {
            _connection = connection;
        }

        public async Task<bool> SendMessage(ConsultantMessage message)
        {
            try
            {
                var result = await _connection.Post("consultantMessages", message);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }
    }
}
