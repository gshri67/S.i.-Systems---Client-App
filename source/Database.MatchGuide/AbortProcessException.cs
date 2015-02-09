using System;
using System.Diagnostics.CodeAnalysis;

namespace SiSystems.ClientApp.Database.MatchGuide
{
    //We don't care to implement serializable just to make the analyzer happy.
    [SuppressMessage("Microsoft.Usage", "CA2237:MarkISerializableTypesWithSerializable")]
    internal class AbortProcessException : Exception
    {
        public AbortProcessException()
        {
        }

        public AbortProcessException(string message) : base(message) { }

        public AbortProcessException(string message, Exception innerException) :
            base(message, innerException)
        {
        }
    }
}
