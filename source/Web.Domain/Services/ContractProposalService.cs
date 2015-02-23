using System;
using SiSystems.ClientApp.SharedModels;
using SiSystems.ClientApp.Web.Domain.Context;
using SiSystems.ClientApp.Web.Domain.Repositories;

namespace SiSystems.ClientApp.Web.Domain.Services
{
    public class ContractProposalService
    {
        private readonly IConsultantRepository _consultantRepository;
        private readonly ICompanyRepository _companyRepository;

        private readonly ISessionContext _sessionContext;

        public ContractProposalService(IConsultantRepository consultantRepository, 
            ICompanyRepository companyRepository, ISessionContext context)
        {
            _consultantRepository = consultantRepository;
            _companyRepository = companyRepository;
            _sessionContext = context;
        }

        public void SendProposal(ContractProposal contract)
        {
            throw new NotImplementedException();
        }
    }
}
