using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiSystems.ClientApp.Web.Domain.Repositories
{
    public class MatchGuideConstants
    {
        public class UserTypes
        {
            public const int Candidate = 490;
            public const int ClientContact = 661;
        }

        public class AgreementTypes
        {
            public const int Contract = 459;
        }

        public class AgreementSubTypes
        {
            public const int FloThru = 172;
        }

        public class ContractStatusTypes
        {
            public const int Active = 573;
            public const int Cancelled = 574;
            public const int Expired = 575;
            public const int Pending = 576;
        }
    }
}
