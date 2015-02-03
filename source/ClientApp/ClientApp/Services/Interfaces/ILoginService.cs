using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SiSystems.ClientApp.SharedModels;

namespace ClientApp.Services.Interfaces
{
    public interface ILoginService
    {
        Task<ValidationResult> LoginAsync(string username, string password);
    }
}
