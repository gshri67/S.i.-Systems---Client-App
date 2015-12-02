﻿using System;
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

        public IEnumerable<string> EmailAddresses { get; set; }
        public IEnumerable<string> PhoneNumbers { get; set; }
    }
}
       