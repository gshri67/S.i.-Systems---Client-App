using System;

namespace SiSystems.ClientApp.SharedModels
{
    public class AccessLevelException : Exception
    {
        public AccessLevelException(string message) : base(message) { }

        public AccessLevelException() : base() { }
    }
}
