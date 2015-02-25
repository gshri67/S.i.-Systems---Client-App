
namespace SiSystems.ClientApp.Web.Domain.Services.EmailTemplates
{
    [Template(IdConfigPropertyName = "Email.ContractProposalTemplateId", Categories="Contract Proposal")]
    public class ContractProposalEmail : TemplatedEmail
    {
        [TemplateSubstitution]
        public string Fee { get; set; }

        [TemplateSubstitution]
        public string RateToConsultant { get; set; }

        [TemplateSubstitution]
        public string TotalRate { get; set; }

        [TemplateSubstitution]
        public string StartDate { get; set; }

        [TemplateSubstitution]
        public string EndDate { get; set; }

        [TemplateSubstitution]
        public string TimesheetApproverEmailAddress { get; set; }

        [TemplateSubstitution]
        public string ClientCompanyName { get; set; }

        [TemplateSubstitution]
        public string ClientContactFullName { get; set; }

        [TemplateSubstitution]
        public string ClientContactEmailAddress { get; set; }

        [TemplateSubstitution]
        public string InvoiceFormat { get; set; }

        [TemplateSubstitution]
        public string Specialization { get; set; }
    }
}
