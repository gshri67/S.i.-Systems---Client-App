using SiSystems.SharedModels;

namespace Shared.Core
{
    /// <summary>
    /// Stores details about the currently logged in user
    /// </summary>
    public static class CurrentConsultantDetails
    {
        private static string _corporationName;
        public static string CorporationName
        {
            get { return _corporationName ?? string.Empty; }
            set { _corporationName = value; } 
        }
    }
}