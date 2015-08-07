namespace SiSystems.SharedModels
{

    public class ClientAccountDetails
    {
        public int ClientId { get; set; }
        public decimal FloThruFee { get; set; }
        public decimal MspFeePercentage { get; set; }
        public decimal MaxVisibleRate { get; set; }
        public MatchGuideConstants.FloThruFeeType FloThruFeeType { get; set; }
        public MatchGuideConstants.FloThruFeePayment FloThruFeePayment { get; set; }
        public MatchGuideConstants.FloThruMspPayment FloThruMspPayment { get; set; }
        public int InvoiceFormat { get; set; }
        public MatchGuideConstants.ClientInvoiceFrequency ClientInvoiceFrequency { get; set; }

        public bool HasAccess { get; set; }
    }
}
