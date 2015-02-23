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
    /// </summary>
    public class MatchGuideConstants
    {
        // Used to identify a type as a match guide constant
        public interface IMatchGuideConstant
        {
             
        }

        public struct UserTypes : IMatchGuideConstant
        {
            public const int Candidate = 490;
            public const int ClientContact = 661;

            private readonly int m_value;

            private UserTypes(int value)
            {
                m_value = value;
            }

            public static implicit operator UserTypes(int val)
            {
                return new UserTypes(val);
            }

            public static implicit operator int(UserTypes s)
            {
                return s.m_value;
            }

            public override string ToString()
            {
                return m_value.ToString();
            }
        }

        public struct AgreementTypes : IMatchGuideConstant
        {
            public const int Contract = 459;

            private readonly int m_value;

            private AgreementTypes(int value)
            {
                m_value = value;
            }

            public static implicit operator AgreementTypes(int val)
            {
                return new AgreementTypes(val);
            }

            public static implicit operator int(AgreementTypes s)
            {
                return s.m_value;
            }

            public override string ToString()
            {
                return m_value.ToString();
            }
        }

        public struct AgreementSubTypes : IMatchGuideConstant
        {
            public const int FloThru = 172;

            private readonly int m_value;

            private AgreementSubTypes(int value)
            {
                m_value = value;
            }

            public static implicit operator AgreementSubTypes(int val)
            {
                return new AgreementSubTypes(val);
            }

            public static implicit operator int(AgreementSubTypes s)
            {
                return s.m_value;
            }

            public override string ToString()
            {
                return m_value.ToString();
            }
        }

        public struct ContractStatusTypes : IMatchGuideConstant
        {
            public const int Active = 573;
            public const int Cancelled = 574;
            public const int Expired = 575;
            public const int Pending = 576;

            private readonly int m_value;

            private ContractStatusTypes(int value)
            {
                m_value = value;
            }

            public static implicit operator ContractStatusTypes(int val)
            {
                return new ContractStatusTypes(val);
            }

            public static implicit operator int(ContractStatusTypes s)
            {
                return s.m_value;
            }

            public override string ToString()
            {
                return m_value.ToString();
            }
        }

        /// Consider any unanticipated values as the default, NotChecked
        public struct ResumeRating : IMatchGuideConstant
        {
            private readonly long m_value;

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

            private static readonly Dictionary<long, string> DescriptionDictionary = new Dictionary<long, string>
            {
                {AboveStandard, "Above Standard"},
                {Standard, "Standard"},
                {BelowStandard, "Below Standard"}
            };

            private const string DefaultDisplayString = "Not Checked";

            private ResumeRating(long value)
            {
                m_value = value;
            }

            public static implicit operator ResumeRating(long val)
            {
                return new ResumeRating(val);
            }

            public static implicit operator long(ResumeRating rating)
            {
                return rating.m_value;
            }

            public override string ToString()
            {
                string description;
                return DescriptionDictionary.TryGetValue(m_value, out description) ? description : DefaultDisplayString;
            }
        }

        public struct YearsOfExperience : IMatchGuideConstant
        {
            public const int LessThanTwo = 448;
            public const int TwoToFour = 449;
            public const int FiveToSeven = 450;
            public const int EightToTen = 451;
            public const int MoreThanTen = 452;

            private static readonly Dictionary<int, string> DescriptionDictionary = new Dictionary<int, string>
            {
                {LessThanTwo, "(< 2 years)"},
                {TwoToFour, "(2-4 years)"},
                {FiveToSeven, "(5-7 years)"},
                {EightToTen, "(8-10 years)"},
                {MoreThanTen, "(> 10 years)"}
            };

            private readonly int m_value;

            private YearsOfExperience(int value)
            {
                m_value = value;
            }

            public static implicit operator YearsOfExperience(int val)
            {
                return new YearsOfExperience(val);
            }

            public static implicit operator int(YearsOfExperience yearsOfExperience)
            {
                return yearsOfExperience.m_value;
            }

            public override string ToString()
            {
                string description;
                return DescriptionDictionary.TryGetValue(m_value, out description) ? description : m_value.ToString();
            }
        }

        public abstract class MatchGuideConstant<T, K>
            where K : MatchGuideConstant<T, K>
        {
            private readonly T m_value;

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
                return (K)Activator.CreateInstance(typeof(K), val);
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
    }
}
