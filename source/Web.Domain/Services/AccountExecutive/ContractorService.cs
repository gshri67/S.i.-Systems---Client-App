using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiSystems.ClientApp.Web.Domain.Repositories.AccountExecutive;
using SiSystems.SharedModels;
using SiSystems.Web.Domain.Context;

namespace SiSystems.ClientApp.Web.Domain.Services.AccountExecutive
{
    public class ContractorService
    {
        private readonly IContractorRepository _repo;
        private readonly ISessionContext _context;
        
        public ContractorService(IContractorRepository repo, ISessionContext context)
        {
            _repo = repo;
            _context = context;
        }

        public Contractor GetContractorById(int id)
        {
            var contractor = _repo.GetContractorById(id);

            AssertCurrentUserCanAccessContractor(contractor);

            return contractor;
        }

        private void AssertCurrentUserCanAccessContractor(Contractor contractor)
        {
            //todo: who isn't allowed to see which contractors?
            if(false)
                throw new UnauthorizedAccessException();
        }
    }
}
