namespace SiSystems.SharedModels
{
    public interface IContractCreationDetails_Sending
    {
        bool IsSendingConsultantContract { get; set; }
        string ClientContactName { get; set; }
        string DirectReportName { get; set; }
        string BillingContactName { get; set; }
        string InvoiceRecipients { get; set; }
        string ClientContractContactName { get; set; }
        bool IsSendingClientContract { get; set; }
        string ReasonForNotSendingContract { get; set; }
        string SummaryReasonForNotSendingContract { get; set; }
    }
}