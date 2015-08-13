using Shared.Core;
using UIKit;
using Xamarin;

namespace ConsultantApp.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            // This will block the startup of the app until any crashes
            // that ocurred within 5 seconds of startup are synced with
            // the server.
            // Note: this will slow down app startup
            Insights.HasPendingCrashReport += (sender, isStartupCrash) =>
            {
                if (isStartupCrash)
                {
                    Insights.PurgePendingCrashReports().Wait();
                }
            };
            Insights.Initialize(Settings.InsightsApiKey);

            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}