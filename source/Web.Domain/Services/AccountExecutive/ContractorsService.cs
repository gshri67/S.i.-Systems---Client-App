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
        private readonly IUserContactRepository _contactRepository;
        private readonly IConsultantContractRepository _contractsRepository;
        
        public ContractorsService(IContractorRepository repo, IUserContactRepository contactRepository, IConsultantContractRepository contractsRepository)
        {
            _repo = repo;
            _contactRepository = contactRepository;
            _contractsRepository = contractsRepository;
        }

        public Contractor GetContractorById(int id)
        {
            var contractor = _repo.GetContractorById(id);
            contractor.ContactInformation = _contactRepository.GetUserContactById(id);
            contractor.Contracts = _contractsRepository.GetContractsByContractorId(id);

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
