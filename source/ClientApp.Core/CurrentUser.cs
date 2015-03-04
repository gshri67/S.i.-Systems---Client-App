namespace ClientApp.Core
{
    /// <summary>
    /// Stores details about the currently logged in user
    /// </summary>
    public static class CurrentUser
    {
        public static string Email { get; set; }

        public static string Domain
        {
            get { return Email != null && Email.Contains("@") ? Email.Substring(Email.IndexOf('@')) : null; }
        }

        public static string User
        {
            get { return Email != null && Email.Contains("@") ? Email.Substring(0, Email.IndexOf('@')) : null; }
        }

        public static decimal ServiceFee { get; set; }
        public static decimal MspPercent { get; set; }
        public static int FloThruFeeType { get; set; }
        public static int FloThruFeePayer { get; set; }
        public static int InvoiceFormat { get; set; }
        public static int InvoiceFrequency { get; set; }
    }
}