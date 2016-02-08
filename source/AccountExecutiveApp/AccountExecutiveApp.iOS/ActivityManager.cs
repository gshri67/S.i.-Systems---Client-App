using Shared.Core.Platform;
using UIKit;

namespace AccountExecutiveApp.iOS
{
    public class ActivityManager : IActivityManager
    {
        private static int _tasks;

        public void StartActivity()
        {
            UIApplication.SharedApplication.NetworkActivityIndicatorVisible = ++_tasks > 0;

        }

        public void StopActivity()
        {
            if (!UIApplication.SharedApplication.NetworkActivityIndicatorVisible || _tasks <= 0)
            {
                _tasks = 0;
                return;
            }
            UIApplication.SharedApplication.NetworkActivityIndicatorVisible = --_tasks > 0;
        }
    }
}