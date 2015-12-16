using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiSystems.SharedModels
{
    public class UserContact
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return string.Format("{0} {1}", FirstName, LastName).Trim(); } }

		public string ClientName { get; set; }
		public string Address { get; set; }

        public IEnumerable<EmailAddress> EmailAddresses { get; set; }
        public IEnumerable<PhoneNumber> PhoneNumbers { get; set; }

        public UserContact()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            ClientName = string.Empty;
            Address = string.Empty;

            EmailAddresses = Enumerable.Empty<EmailAddress>();
            PhoneNumbers = Enumerable.Empty<PhoneNumber>();
        }
    }

    public class EmailAddress
    {
        public string Email { get; set; }
        public string Title { get; set; }
    }

    public class PhoneNumber
    {
        public string Title { get; set; }
        public int AreaCode { get; set; }
        public int Prefix { get; set; }
        public int LineNumber { get; set; }
        public int Extension { get; set; }

        public string FormattedNumber
        {
            get { return string.Format("({0}){1}-{2}", AreaCode, Prefix, LineNumber); }
        }
    }
}
       