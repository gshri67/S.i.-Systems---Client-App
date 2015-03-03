using System.Linq;

namespace ClientApp
{
    /// <summary>
    /// Stores details about the currently logged in user
    /// </summary>
    public static class CurrentUser
    {
        public static string Email { get; set; }

        public static string Domain
        {
            get { return Email.Contains('@') ? Email.Substring(Email.IndexOf('@')) : null; }
        }

        public static string User
        {
            get { return Email.Contains('@') ? Email.Substring(0, Email.IndexOf('@')) : null; }
        }

        public const decimal ServiceFee = 3;
        public const decimal MspPercent = 0;
    }
}