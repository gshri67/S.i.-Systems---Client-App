using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SiSystems.ClientApp.SharedModels;

namespace ClientApp.Services.Interfaces
{
    public interface IConnectionService
    {
        Task<ValidationResult> Login(string username, string password);
        Task<string> Get(string service);
        Task<string> Get(string service, string query);
        Task<string> Post(string service, object data);
        OAuthToken Token { get; set; }
    }
}
