using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiSystems.ClientApp.SharedModels
{
    public class User
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public string CompanyName { get; set; }
     
        public string Login { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName { get { return string.Format("{0} {1}", FirstName, LastName); } }

        public string PasswordHash { get; set; }

        public MatchGuideConstants.Role UserRole { get; set; }
        public MatchGuideConstants.UserType UserType { get; set; }

        public MatchGuideConstants.ClientPortalType ClientPortalType { get; set; }
        
        /// <summary>
        /// Determines level of access to the Alumni app
        /// </summary>
        public MatchGuideConstants.FloThruAlumniAccess FloThruAlumniAccess { get; set; }
    }
}
