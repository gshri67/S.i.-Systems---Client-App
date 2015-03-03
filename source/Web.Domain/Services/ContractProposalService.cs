using SiSystems.ClientApp.SharedModels;
using SiSystems.ClientApp.Web.Domain.Context;
using SiSystems.ClientApp.Web.Domain.Repositories;
using SiSystems.ClientApp.Web.Domain.Services.EmailTemplates;

namespace SiSystems.ClientApp.Web.Domain.Services
{
    public class ContractProposalService
    {
        private readonly IConsultantRepository _consultantRepository;

        private readonly ISessionContext _sessionContext;

        private readonly SendGridMailService _mailService;

        public ContractProposalService(IConsultantRepository consultantRepository, ISessionContext context, SendGridMailService mailService)
        {
            _consultantRepository = consultantRepository;
            _sessionContext = context;
            _mailService = mailService;
        }

        public void SendProposal(ContractProposal proposal)
        {
            var consultant = _consultantRepository.Find(proposal.ConsultantId);

            var email = new ContractProposalEmail
            {
                To = consultant.EmailAddress,
                From = _sessionContext.CurrentUser.Login,
                ClientCompanyName = _sessionContext.CurrentUser.CompanyName,
                ClientContactFullName = _sessionContext.CurrentUser.FullName,
                ClientContactEmailAddress = _sessionContext.CurrentUser.Login,
                // TODO: Invoice Format
                InvoiceFormat = string.Empty,
                
                Fee = proposal.Fee.ToString("C"),
                RateToConsultant = proposal.RateToConsultant.ToString("C"),
                TotalRate = string.Format("{0:C}", proposal.Fee + proposal.RateToConsultant),
                StartDate = proposal.StartDate.ToShortDateString(),
                EndDate = proposal.EndDate.ToShortDateString(),
                TimesheetApproverEmailAddress = proposal.TimesheetApproverEmailAddress,
                ContractApproverEmailAddress = proposal.ContractApproverEmailAddress,
            };

            _mailService.SendTemplatedEmail(email);
        }
    }
}
