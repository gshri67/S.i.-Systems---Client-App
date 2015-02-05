using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Services.Interfaces
{
    public interface IConnectionService
    {
        Task<bool> Login(string username, string password);
        Task<string> Get(string service);
        Task<string> Post(string service, object data);
    }
}
