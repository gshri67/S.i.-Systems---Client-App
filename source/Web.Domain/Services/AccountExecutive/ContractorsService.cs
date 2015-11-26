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
    public class ContractorsService
    {
        private readonly IContractorRepository _repo;
        private readonly ISessionContext _context;
        
        public ContractorsService(IContractorRepository repo, ISessionContext context)
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

        public IEnumerable<Contractor> GetContractorsByJobIdAndStatus(int id, JobStatus status)
        {
            IEnumerable<Contractor> contractors = Enumerable.Empty<Contractor>();

            if( status == JobStatus.Shortlisted )
                contractors = _repo.GetShortlistedContractorsByJobId(id);
            else if (status == JobStatus.Proposed)
                contractors = _repo.GetProposedContractorsByJobId(id);
            else if (status == JobStatus.Callout)
                contractors = _repo.GetCalloutContractorsByJobId(id);

            return contractors;
        }
    }
}
