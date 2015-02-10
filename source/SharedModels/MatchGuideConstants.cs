namespace SiSystems.ClientApp.SharedModels
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

        public class ResumeRating
        {
            public const int AboveStandard = 314;
            public const int Standard = 315;
            public const int BelowStandard = 316;

            public const int NotChecked = 317;
            
            ///this value appears as a default in the database
            ///but normally is used as a lookup for User Type = 'Contact'
            ///consider this equivalent to NotChecked
            public const int AlsoNotChecked = 5;
        }
    }
}
