using System;
using System.Collections.Generic;

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
    /// 
    /// TODO: might be worth changing these to structs so that the have to
    /// explicitly be delclared nullable
    /// </summary>
    public class MatchGuideConstants
    {
        public abstract class MatchGuideConstant<T,K>
            where K: MatchGuideConstant<T,K>
        {
            private readonly T m_value;
            private bool hasValue = false;

            protected virtual Dictionary<T, string> DescriptionDictionary { get { return new Dictionary<T, string>(); } }

            /// <summary>
            /// Provides the default text to display if the value is not defined
            /// as a key in the the DescriptionDictionary
            /// </summary>
            protected virtual Func<T, string> DefaultStringFunc
            {
                get { return arg => arg.ToString(); }
            }

            protected MatchGuideConstant()
            {
            }

            protected MatchGuideConstant(T value)
            {
                m_value = value;
            }

            protected T GetBackingValue()
            {
                return m_value;
            }

            public static implicit operator MatchGuideConstant<T, K>(T val)
            {
                return (K) Activator.CreateInstance(typeof (K), val);
            }

            public static implicit operator T(MatchGuideConstant<T, K> matchGuideConstant)
            {
                return matchGuideConstant.GetBackingValue();
            }

            public override string ToString()
            {
                string description;
                return DescriptionDictionary.TryGetValue(m_value, out description) ? description : DefaultStringFunc(m_value);
            }
        }

        public class UserTypes : MatchGuideConstant<int, UserTypes>
        {
            public const int Candidate = 490;
            public const int ClientContact = 661;
        }

        public class AgreementTypes : MatchGuideConstant<int, AgreementTypes>
        {
            public const int Contract = 459;
        }

        public class AgreementSubTypes : MatchGuideConstant<int, AgreementSubTypes>
        {
            public const int FloThru = 172;
        }

        public class ContractStatusTypes: MatchGuideConstant<int, ContractStatusTypes>
        {
            public const int Active = 573;
            public const int Cancelled = 574;
            public const int Expired = 575;
            public const int Pending = 576;
        }

        /// Consider any unanticipated values as the default, NotChecked
        public class ResumeRating : MatchGuideConstant<int, ResumeRating>
        {
            public const int AboveStandard = 314;
            public const int Standard = 315;
            public const int BelowStandard = 316;
            public const int NotChecked = 317;

            /// <summary>
            /// this value appears as an incorrect default in the database
            /// which normally is used as a lookup for UserType = 'Contact'
            /// consider this equivalent to NotChecked
            /// </summary>
            public const int AlsoNotChecked = 5;

            protected override Func<int, string> DefaultStringFunc
            {
                get { return val => "Not Checked"; }
            }

            private static readonly Dictionary<int, string> _descriptionDictionary = new Dictionary<int, string>
            {
                {AboveStandard, "Above Standard"},
                {Standard, "Standard"},
                {BelowStandard, "Below Standard"}
            };

            protected override Dictionary<int, string> DescriptionDictionary
            {
                get { return _descriptionDictionary; }
            }
        }

        public class YearsOfExperience : MatchGuideConstant<int, YearsOfExperience>
        {
            public const int LessThanTwo = 448;
            public const int TwoToFour = 449;
            public const int FiveToSeven = 450;
            public const int EightToTen = 451;
            public const int MoreThanTen = 452;

            private static readonly Dictionary<int, string> _descriptionDictionary = new Dictionary<int, string>
            {
                {LessThanTwo, "(< 2 years)"},
                {TwoToFour, "(2-4 years)"},
                {FiveToSeven, "(5-7 years)"},
                {EightToTen, "(8-10 years)"},
                {MoreThanTen, "(> 10 years)"}
            };

            protected override Dictionary<int, string> DescriptionDictionary
            {
                get { return _descriptionDictionary; }
            }
        }
    }
}
