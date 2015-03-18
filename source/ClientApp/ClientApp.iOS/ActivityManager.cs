using System;
using UIKit;
using ClientApp.Core;
using System.Threading;
using System.Threading.Tasks;

namespace ClientApp.iOS
{
    public class ActivityManager : IActivityManager
    {
        private static int _tasks;

        public void StartActivity()
        {
            StartActivity(CancellationToken.None);
        }

        public Guid StartActivity(CancellationToken cancellationToken)
        {

            UIApplication.SharedApplication.NetworkActivityIndicatorVisible = ++_tasks > 0;

            return Guid.Empty;
        }

        public void StopActivity()
        {
            StopActivity(Guid.Empty);
        }

        public void StopActivity(Guid activityId)
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