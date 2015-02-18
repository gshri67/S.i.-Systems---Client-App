namespace SiSystems.ClientApp.SharedModels
{
    /// <summary>
    /// Constants are currently const ints over enums 
    /// as values returned from database may not be entirely predictable.
    /// There is a risk that unanticipated values may be encountered.
    /// 
    /// For example, ResumeRating has invalid values (5) in the database currently.
    /// Enums could be used if the domain layer maintains its own 'database models'
    /// and insulates callers from these details, but by adding additional complexity.
    /// </summary>
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

        /// Consider any unanticipated values as the default, NotChecked
        public class ResumeRating
        {
            public const int AboveStandard = 314;
            public const int Standard = 315;
            public const int BelowStandard = 316;

            public const int NotChecked = 317;
            
            ///this value appears as an incorrect default in the database
            ///which normally is used as a lookup for UserType = 'Contact'
            ///consider this equivalent to NotChecked
            public const int AlsoNotChecked = 5;
        }

        public class YearsOfExperience
        {
            public const int LessThanTwo = 448;
            public const int TwoToFour = 449;
            public const int FiveToSeven = 450;
            public const int EightToTen = 451;
            public const int MoreThanTen = 452;
        }
    }
}
