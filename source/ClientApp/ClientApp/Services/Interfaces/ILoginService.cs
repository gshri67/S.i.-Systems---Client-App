using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Services.Interfaces
{
    public interface ILoginService
    {
        Task<bool> LoginAsync(string username, string password);
    }
}
