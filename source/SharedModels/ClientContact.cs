using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiSystems.SharedModels
{
    public class ClientContact
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return string.Format("{0} {1}", FirstName, LastName).Trim(); } }

        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set;  }
    }
}
       