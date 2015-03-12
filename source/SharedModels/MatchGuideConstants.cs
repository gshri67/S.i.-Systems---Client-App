using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Xml.Serialization;

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

        /// <summary>
        /// Corresponds to PickTypeId = 53 in the 
        /// PickListTable in the MatchGuide database.
        /// </summary>
        [DataContract]
        [DebuggerDisplay("{DisplayValue}")]
        public struct UserType : IMatchGuideConstant
        {
            [DataMember(Name = "backingValue")]
            private readonly long m_value;

            [DataMember(Name = "displayValue")]
            private string DisplayValue
            {
                get { return this.ToString(); }
            }

            public const int Candidate = 490;
            public const int ClientContact = 491;
            public const int Unknown = 492;
            public const int CareerCandidate = 514;
            public const int InternalUser = 693;
            public const int NewCandidate = 2385;

            private UserType(long value)
            {
                m_value = value;
            }

            public static implicit operator UserType(long val)
            {
                return new UserType(val);
            }

            public static implicit operator UserType(int val)
            {
                return new UserType(val);
            }

            public static implicit operator long(UserType s)
            {
                return s.m_value;
            }

            public override string ToString()
            {
                return m_value.ToString();
            }
        }

        /// <summary>
        /// Corresponds to PickTypeId = 105 in the 
        /// PickListTable in the MatchGuide database.
        /// </summary>
        [DataContract]
        [DebuggerDisplay("{DisplayValue}")]
        public struct ClientPortalType : IMatchGuideConstant
        {
            [DataMember(Name = "backingValue")]
            private readonly long m_value;

            [DataMember(Name = "displayValue")]
            private string DisplayValue
            {
                get { return this.ToString(); }
            }

            public const int PortalContact = 833;
            public const int PortalAdministrator = 834;
            public const int NoPortalAccess = 835;

            /// <summary>
            /// this value appears as an incorrect default in the database
            /// which normally is used as a lookup for UserType = 'Contact'
            /// consider this equivalent to NotChecked
            /// </summary>
            public const int AlsoNotChecked = 5;

            private static readonly Dictionary<long, string> DescriptionDictionary = new Dictionary<long, string>
            {
                {PortalContact, "Portal Contact"},
                {PortalAdministrator, "Portal Administrator"},
                {NoPortalAccess, "No Portal Access"}
            };

            private const string DefaultDisplayString = "No Portal Access";

            private ClientPortalType(long value)
            {
                m_value = value;
            }

            public static implicit operator ClientPortalType(long val)
            {
                return new ClientPortalType(val);
            }

            public static implicit operator ClientPortalType(int val)
            {
                return new ClientPortalType(val);
            }

            public static implicit operator long (ClientPortalType rating)
            {
                return rating.m_value;
            }

            public override string ToString()
            {
                string description;
                return DescriptionDictionary.TryGetValue(m_value, out description) ? description : DefaultDisplayString;
            }
        }

        /// <summary>
        /// Corresponds to PickTypeId = 168 in the 
        /// PickListTable in the MatchGuide database.
        /// </summary>
        [DataContract]
        [DebuggerDisplay("{DisplayValue}")]
        public struct FloThruAlumniAccess : IMatchGuideConstant
        {
            [DataMember(Name = "backingValue")]
            private readonly long m_value;

            [DataMember(Name = "displayValue")]
            private string DisplayValue
            {
                get { return this.ToString(); }
            }

            public const int NoAccess = 2467;
            public const int LimitedAccess = 2468;
            public const int AllAccess = 2469;

            /// <summary>
            /// this value appears as an incorrect default in the database
            /// which normally is used as a lookup for UserType = 'Contact'
            /// consider this equivalent to NotChecked
            /// </summary>
            public const int AlsoNotChecked = 5;

            private static readonly Dictionary<long, string> DescriptionDictionary = new Dictionary<long, string>
            {
                {NoAccess, "No Access"},
                {LimitedAccess, "Limited Access"},
                {AllAccess, "All Access"}
            };

            private const string DefaultDisplayString = "No Access";

            private FloThruAlumniAccess(long value)
            {
                m_value = value;
            }

            public static implicit operator FloThruAlumniAccess(long val)
            {
                return new FloThruAlumniAccess(val);
            }

            public static implicit operator FloThruAlumniAccess(int val)
            {
                return new FloThruAlumniAccess(val);
            }

            public static implicit operator long (FloThruAlumniAccess rating)
            {
                return rating.m_value;
            }

            public override string ToString()
            {
                string description;
                return DescriptionDictionary.TryGetValue(m_value, out description) ? description : DefaultDisplayString;
            }
        }

        /// <summary>
        /// Corresponds to PickTypeId = 45 in the 
        /// PickListTable in the MatchGuide database.
        /// </summary>
        [DataContract]
        [DebuggerDisplay("{DisplayValue}")]
        public struct AgreementTypes : IMatchGuideConstant
        {
            [DataMember(Name = "backingValue")]
            private readonly long m_value;

            [DataMember(Name = "displayValue")]
            private string DisplayValue
            {
                get { return this.ToString(); }
            }

            public const int Opportunity = 458;
            public const int Contract = 459;
            public const int RFP = 460;
            public const int SA = 462;
            public const int RFPOpp = 992;
            public const int TA = 998;

            private AgreementTypes(long value)
            {
                m_value = value;
            }

            public static implicit operator AgreementTypes(long val)
            {
                return new AgreementTypes(val);
            }

            public static implicit operator long(AgreementTypes s)
            {
                return s.m_value;
            }

            public override string ToString()
            {
                return m_value.ToString();
            }
        }

        /// <summary>
        /// Corresponds to PickTypeId = 21 in the 
        /// PickListTable in the MatchGuide database.
        /// </summary>
        [DataContract]
        [DebuggerDisplay("{DisplayValue}")]
        public struct AgreementSubTypes : IMatchGuideConstant
        {
            [DataMember(Name = "backingValue")]
            private readonly long m_value;

            [DataMember(Name = "displayValue")]
            private string DisplayValue
            {
                get { return this.ToString(); }
            }

            public const int Consultant = 171;
            public const int FloThru = 172;
            public const int SO_PVL = 173;
            public const int Permanent = 174;
            public const int ContractToHire = 175;

            private AgreementSubTypes(long value)
            {
                m_value = value;
            }

            public static implicit operator AgreementSubTypes(long val)
            {
                return new AgreementSubTypes(val);
            }

            public static implicit operator AgreementSubTypes(int val)
            {
                return new AgreementSubTypes(val);
            }

            public static implicit operator long(AgreementSubTypes s)
            {
                return s.m_value;
            }

            public override string ToString()
            {
                return m_value.ToString();
            }
        }

        /// <summary>
        /// Corresponds to PickTypeId = 66 in the 
        /// PickListTable in the MatchGuide database.
        /// </summary>
        [DataContract]
        [DebuggerDisplay("{DisplayValue}")]
        public struct ContractStatusTypes : IMatchGuideConstant
        {
            [DataMember(Name = "backingValue")]
            private readonly long m_value;

            [DataMember(Name = "displayValue")]
            private string DisplayValue
            {
                get { return this.ToString(); }
            }

            public const int Active = 573;
            public const int Cancelled = 574;
            public const int Expired = 575;
            public const int Pending = 576;

            private ContractStatusTypes(long value)
            {
                m_value = value;
            }

            public static implicit operator ContractStatusTypes(long val)
            {
                return new ContractStatusTypes(val);
            }

            public static implicit operator long(ContractStatusTypes s)
            {
                return s.m_value;
            }

            public override string ToString()
            {
                return m_value.ToString();
            }
        }

        /// Consider any unanticipated values as the default, NotChecked
        [DataContract]
        [DebuggerDisplay("{DisplayValue}")]
        public struct ResumeRating : IMatchGuideConstant
        {
            [DataMember(Name = "backingValue")]
            private readonly long m_value;

            [DataMember(Name = "displayValue")]
            private string DisplayValue
            {
                get { return this.ToString(); }
            }

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

            public static implicit operator ResumeRating(int val)
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

        [DataContract]
        [DebuggerDisplay("{DisplayValue}")]
        public struct YearsOfExperience : IMatchGuideConstant
        {
            [DataMember(Name = "backingValue")]
            private readonly long m_value;

            [DataMember(Name = "displayValue")]
            private string DisplayValue
            {
                get { return this.ToString(); }
            }

            public const int LessThanTwo = 448;
            public const int TwoToFour = 449;
            public const int FiveToSeven = 450;
            public const int EightToTen = 451;
            public const int MoreThanTen = 452;

            private static readonly Dictionary<long, string> DescriptionDictionary = new Dictionary<long, string>
            {
                {LessThanTwo, "(< 2 years)"},
                {TwoToFour, "(2-4 years)"},
                {FiveToSeven, "(5-7 years)"},
                {EightToTen, "(8-10 years)"},
                {MoreThanTen, "(> 10 years)"}
            };

            private YearsOfExperience(long value)
            {
                m_value = value;
            }

            public static implicit operator YearsOfExperience(long val)
            {
                return new YearsOfExperience(val);
            }

            public static implicit operator YearsOfExperience(int val)
            {
                return new YearsOfExperience(val);
            }

            public static implicit operator long(YearsOfExperience yearsOfExperience)
            {
                return yearsOfExperience.m_value;
            }

            public override string ToString()
            {
                string description;
                return DescriptionDictionary.TryGetValue(m_value, out description) ? description : m_value.ToString();
            }
        }

        public abstract class MatchGuideConstant<TBackingType, TConstantType>
            where TConstantType : MatchGuideConstant<TBackingType, TConstantType>
        {
            private readonly TBackingType m_value;

            protected virtual Dictionary<TBackingType, string> DescriptionDictionary { get { return new Dictionary<TBackingType, string>(); } }

            /// <summary>
            /// Provides the default text to display if the value is not defined
            /// as a key in the the DescriptionDictionary
            /// </summary>
            protected virtual Func<TBackingType, string> DefaultStringFunc
            {
                get { return arg => arg.ToString(); }
            }

            protected MatchGuideConstant()
            {
            }

            protected MatchGuideConstant(TBackingType value)
            {
                m_value = value;
            }

            protected TBackingType GetBackingValue()
            {
                return m_value;
            }

            public static implicit operator MatchGuideConstant<TBackingType, TConstantType>(TBackingType val)
            {
                return (TConstantType)Activator.CreateInstance(typeof(TConstantType), val);
            }

            public static implicit operator TBackingType(MatchGuideConstant<TBackingType, TConstantType> matchGuideConstant)
            {
                return matchGuideConstant.GetBackingValue();
            }

            public override string ToString()
            {
                string description;
                return DescriptionDictionary.TryGetValue(m_value, out description) ? description : DefaultStringFunc(m_value);
            }
        }


        [DataContract]
        [DebuggerDisplay("{DisplayValue}")]
        public struct FloThruFeePayment : IMatchGuideConstant
        {
            [DataMember(Name = "backingValue")]
            private readonly long m_value;

            [DataMember(Name = "displayValue")]
            private string DisplayValue
            {
                get { return this.ToString(); }
            }

            public const int ClientPays = 2463;
            public const int ContractorPays = 2464;

            private static readonly Dictionary<long, string> DescriptionDictionary = new Dictionary<long, string>
            {
                {ClientPays, "Client Pays"},
                {ContractorPays, "Contractor Pays"},
            };

            private const string DefaultDisplayString = "Client Pays";

            private FloThruFeePayment(long value)
            {
                m_value = value;
            }

            public static implicit operator FloThruFeePayment(long val)
            {
                return new FloThruFeePayment(val);
            }

            public static implicit operator FloThruFeePayment(int val)
            {
                return new FloThruFeePayment(val);
            }

            public static implicit operator long(FloThruFeePayment feePayment)
            {
                return feePayment.m_value;
            }

            public override string ToString()
            {
                string description;
                return DescriptionDictionary.TryGetValue(m_value, out description) ? description : DefaultDisplayString;
            }
        }

        [DataContract]
        [DebuggerDisplay("{DisplayValue}")]
        public struct FloThruFeeType : IMatchGuideConstant
        {
            [DataMember(Name = "backingValue")]
            private readonly long m_value;

            [DataMember(Name = "displayValue")]
            private string DisplayValue
            {
                get { return this.ToString(); }
            }

            public const int Percentage = 2460;
            public const int DollarsPerHour = 2461;
            public const int DollarsPerDiem = 2462;

            private static readonly Dictionary<long, string> DescriptionDictionary = new Dictionary<long, string>
            {
                {Percentage, "Percentage"},
                {DollarsPerHour, "Dollars per Hour"},
                {DollarsPerDiem, "Dollars per Diem"},
            };

            private const string DefaultDisplayString = "Dollars per Hour";

            private FloThruFeeType(long value)
            {
                m_value = value;
            }

            public static implicit operator FloThruFeeType(long val)
            {
                return new FloThruFeeType(val);
            }

            public static implicit operator FloThruFeeType(int val)
            {
                return new FloThruFeeType(val);
            }

            public static implicit operator long(FloThruFeeType feeType)
            {
                return feeType.m_value;
            }

            public override string ToString()
            {
                string description;
                return DescriptionDictionary.TryGetValue(m_value, out description) ? description : DefaultDisplayString;
            }
        }

        [DataContract]
        [DebuggerDisplay("{DisplayValue}")]
        public struct ClientInvoiceFrequency : IMatchGuideConstant
        {
            [DataMember(Name = "backingValue")]
            private readonly long m_value;

            [DataMember(Name = "displayValue")]
            private string DisplayValue
            {
                get { return this.ToString(); }
            }

            public const int Monthly = 956;
            public const int SemiMonthly = 957;
            public const int Weekly = 973;
            public const int InactiveWeekly = 978;

            private static readonly Dictionary<long, string> DescriptionDictionary = new Dictionary<long, string>
            {
                {Monthly, "Monthly"},
                {SemiMonthly, "Semi - Monthly"},
                {Weekly, "Weekly"},
                {InactiveWeekly, "Weekly"},
            };

            private const string DefaultDisplayString = "Monthly";

            private ClientInvoiceFrequency(long value)
            {
                m_value = value;
            }

            public static implicit operator ClientInvoiceFrequency(long val)
            {
                return new ClientInvoiceFrequency(val);
            }

            public static implicit operator ClientInvoiceFrequency(int val)
            {
                return new ClientInvoiceFrequency(val);
            }

            public static implicit operator long(ClientInvoiceFrequency frequency)
            {
                return frequency.m_value;
            }

            public override string ToString()
            {
                string description;
                return DescriptionDictionary.TryGetValue(m_value, out description) ? description : DefaultDisplayString;
            }
        }
        
        [DataContract]
        [DebuggerDisplay("{DisplayValue}")]
        public struct FloThruMspPayment : IMatchGuideConstant
        {
            [DataMember(Name = "backingValue")]
            private readonly long m_value;

            [DataMember(Name = "displayValue")]
            private string DisplayValue
            {
                get { return this.ToString(); }
            }

            public const int AddToBillRate = 2465;
            public const int DeductFromContractorPay = 2466;

            private static readonly Dictionary<long, string> DescriptionDictionary = new Dictionary<long, string>
            {
                {AddToBillRate, "Add to Bill Rate"},
                {DeductFromContractorPay, "Deduct from Contractor Pay"},
            };

            private const string DefaultDisplayString = "Add to Bill Rate";

            private FloThruMspPayment(long value)
            {
                m_value = value;
            }

            public static implicit operator FloThruMspPayment(long val)
            {
                return new FloThruMspPayment(val);
            }

            public static implicit operator FloThruMspPayment(int val)
            {
                return new FloThruMspPayment(val);
            }

            public static implicit operator long(FloThruMspPayment mspPayment)
            {
                return mspPayment.m_value;
            }

            public override string ToString()
            {
                string description;
                return DescriptionDictionary.TryGetValue(m_value, out description) ? description : DefaultDisplayString;
            }
        }
    }
}
