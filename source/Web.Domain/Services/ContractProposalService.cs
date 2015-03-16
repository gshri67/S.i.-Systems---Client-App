using System.Globalization;
using System.Text;
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

                ConsultantName = consultant.FullName,
                JobTitle = proposal.JobTitle,
                ClientCompanyName = _sessionContext.CurrentUser.CompanyName,
                ClientContactFullName = _sessionContext.CurrentUser.FullName,
                ClientContactEmailAddress = _sessionContext.CurrentUser.Login,
                // TODO: unsure of the type of data we're getting from MatchGuide for InvoiceFormat
                InvoiceFormat = proposal.InvoiceFormat.ToString(),
                
                Fee = BuildFeeString(proposal),
                RateToConsultant = CalculateRate(proposal),
                TotalRate = string.Format("{0:C}", proposal.Fee + proposal.Rate),
                StartDate = proposal.StartDate.ToShortDateString(),
                EndDate = proposal.EndDate.ToShortDateString(),
                TimesheetApproverEmailAddress = proposal.TimesheetApproverEmailAddress,
                ContractApproverEmailAddress = proposal.ContractApproverEmailAddress,
                //todo: to send or not to send. Add if this gets sent in the email
                //MspFeePercentage = proposal.MspFeePercentage.ToString(CultureInfo.InvariantCulture)
            };

            _mailService.SendTemplatedEmail(email);
        }

        private string BuildFeeString(ContractProposal proposal)
        {
            var sb = new StringBuilder();
            if (proposal.MspFeePercentage > 0)
            {
                var amount = proposal.Rate*(proposal.MspFeePercentage/100);
                if (proposal.FloThruMspPayment == MatchGuideConstants.FloThruMspPayment.DeductFromContractorPay)
                {
                    sb.Append("Contractor Pays ");
                }
                sb.Append("MSP ").Append(proposal.MspFeePercentage).Append("% (").Append(amount.ToString("C")).Append(")");
            }
            if (sb.Length > 0)
            {
                sb.Append("+ ");
            }
            if (proposal.Fee > 0)
            {
                if (proposal.FloThruFeePayment == MatchGuideConstants.FloThruFeePayment.ContractorPays)
                {
                    sb.Append("Contractor Pays ");
                }
                sb.Append("").Append(proposal.Fee.ToString("C")).Append("Service ");
            }
            if (sb.Length == 0)
            {
                sb.Append("$0.00");
            }
            return sb.ToString();
        }

        private string CalculateRate(ContractProposal proposal)
        {
            var rate = proposal.Rate;
            if (proposal.MspFeePercentage > 0 && proposal.FloThruMspPayment == MatchGuideConstants.FloThruMspPayment.DeductFromContractorPay)
            {
                rate = (rate*(1 - proposal.MspFeePercentage));
            }
            
            if (proposal.Fee > 0 && proposal.FloThruFeePayment == MatchGuideConstants.FloThruFeePayment.ContractorPays)
            {
                rate = rate - proposal.Fee;
            }
            return rate.ToString("C");
        }
    }
}
