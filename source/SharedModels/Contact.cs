using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiSystems.ClientApp.SharedModels
{
    public class Contact
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return FirstName + " " + LastName; } }

        public string EmailAddress { get; set; }
    }
}
