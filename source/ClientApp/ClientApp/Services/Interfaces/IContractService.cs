using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SiSystems.ClientApp.SharedModels;

namespace ClientApp.Services.Interfaces
{
    public interface IContractService
    {
        Task<IEnumerable<Specialization>> GetAllSpecializations();
        //private Task<ValidationResult> Submit(NewContractObject);
    }
}
