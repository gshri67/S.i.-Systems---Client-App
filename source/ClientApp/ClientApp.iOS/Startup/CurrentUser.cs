using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using Foundation;
using UIKit;

namespace ClientApp.iOS.Startup
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
    }
}