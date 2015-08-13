using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundation;
using UIKit;
using Xamarin;

namespace ConsultantApp.iOS
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