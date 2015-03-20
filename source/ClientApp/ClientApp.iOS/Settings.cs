using Foundation;
using System.Configuration;
using Xamarin;

namespace ClientApp.iOS
{
    public static class Settings
    {
        public static string InsightsApiKey
        {
            get 
            { 
                var configValue = NSBundle.MainBundle.ObjectForInfoDictionary("SIAlumniXamarinInsightsAPIKey") as NSString; 
                return string.IsNullOrEmpty(configValue) ? Insights.DebugModeKey : configValue; 
            }
        }
    }
}