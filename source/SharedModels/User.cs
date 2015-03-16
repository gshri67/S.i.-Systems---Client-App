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

        public int CompanyId { get; set; }
     
        public string Login { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName { get { return string.Format("{0} {1}", FirstName, LastName); } }

        public string PasswordHash { get; set; }

        public MatchGuideConstants.UserType UserType { get; set; }

        public MatchGuideConstants.FloThruAlumniAccess FloThruAlumniAccess { get; set; }

        public MatchGuideConstants.ClientPortalType ClientPortalType { get; set; }

        public string CompanyName { get; set; }

        public bool IsCompanyParticipating { get; set; }

        public decimal? ClientsMaxVisibleRate { get; set; }
    }
}
