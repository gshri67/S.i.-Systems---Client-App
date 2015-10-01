using SiSystems.SharedModels;

namespace Shared.Core
{
    /// <summary>
    /// Stores details about the currently logged in user
    /// </summary>
    public static class CurrentConsultantDetails
    {
        public static string CorporationName
        {
            get { return CorporationName ?? string.Empty; } 
            set { CorporationName = value; } 
        }
    }
}