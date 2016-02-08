using Foundation;
using Xamarin;

namespace AccountExecutiveApp.iOS
{
    public class Settings
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